using CourseManagementService.DTOs;
using CourseManagementService.Models;
using Microsoft.AspNetCore.Mvc;
using NReco.VideoInfo;
using System.Diagnostics;

namespace CourseManagementService.Service
{
    public class VideoProcessingService
    {
        private IConfiguration _configuration;
        private ILogger<VideoProcessingService> _logger;
        private string _videoStoragePath;
        private FFProbe _ffProbe;

        public VideoProcessingService(
            IConfiguration configuration,
            ILogger<VideoProcessingService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _videoStoragePath = configuration["VideoStorage:Path"] ?? Path.Combine(Directory.GetCurrentDirectory(), "Videos");
            _ffProbe = new FFProbe(); // Khởi tạo FFProbe từ NReco.VideoInfo.LT

            // Tạo thư mục lưu trữ video nếu chưa tồn tại
            Directory.CreateDirectory(_videoStoragePath);
        }

        public async Task<string> UploadVideoAsync(IFormFile videoFile)
        {
            // Kiểm tra file
            if (videoFile == null || videoFile.Length == 0)
                throw new ArgumentException("Video file is empty");

            // Kiểm tra kích thước file
            if (videoFile.Length > 500 * 1024 * 1024) // Giới hạn 500MB
                throw new ArgumentException("File size exceeds 500MB limit");

            // Tạo tên file duy nhất
            string fileName = $"{Guid.NewGuid()}_{videoFile.FileName}";
            string filePath = Path.Combine(_videoStoragePath, fileName);

            // Lưu file gốc
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            // Xử lý video với FFmpeg
            await ProcessVideoAsync(filePath);

            return fileName;
        }

        private async Task ProcessVideoAsync(string inputFilePath)
        {
            try
            {
                // Lấy thông tin video bằng NReco.VideoInfo
                var videoInfo = _ffProbe.GetMediaInfo(inputFilePath);
                var videoStream = videoInfo.Streams?.FirstOrDefault(s => s.CodecType == "video");

                if (videoStream == null)
                    throw new Exception("File không chứa stream video");

                // Ghi log thông tin video
                _logger.LogInformation($"Video info: Duration={videoInfo.Duration}, " +
                    $"Resolution={videoStream.Width}x{videoStream.Height}, " +
                    $"Format={videoInfo.FormatName}");

                // Tạo thư mục cho các phiên bản video
                string outputDirectory = Path.Combine(_videoStoragePath, Path.GetFileNameWithoutExtension(inputFilePath));
                Directory.CreateDirectory(outputDirectory);

                // Các preset chất lượng video (tự động điều chỉnh dựa trên độ phân giải gốc)
                var qualities = GetQualityPresets(videoStream.Width, videoStream.Height);

                foreach (var quality in qualities)
                {
                    string outputFilePath = Path.Combine(outputDirectory, $"video_{quality.Resolution}.mp4");

                    // Câu lệnh FFmpeg để encode video
                    string ffmpegCommand = $"-i \"{inputFilePath}\" " +
                        $"-vf scale=-2:{quality.Resolution.TrimEnd('p')} " +
                        $"-c:v libx264 -preset medium -crf 23 -b:v {quality.BitrateVideo} " +
                        $"-c:a aac -b:a {quality.BitrateAudio} " +
                        $"\"{outputFilePath}\"";

                    await RunFFmpegCommandAsync(ffmpegCommand);
                }

                // Tạo thumbnail
                string thumbnailPath = Path.Combine(outputDirectory, "thumbnail.jpg");
                string thumbnailCommand = $"-i \"{inputFilePath}\" -ss 00:00:05 -vframes 1 \"{thumbnailPath}\"";
                await RunFFmpegCommandAsync(thumbnailCommand);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Video processing error: {ex.Message}");
                throw;
            }
        }

        private List<VideoQualityPreset> GetQualityPresets(int originalWidth, int originalHeight)
        {
            var presets = new List<VideoQualityPreset>();

            // Xác định các preset phù hợp dựa trên độ phân giải gốc
            if (originalHeight >= 720)
            {
                presets.Add(new VideoQualityPreset("720p", 720, "2500k", "256k"));
            }

            if (originalHeight >= 480)
            {
                presets.Add(new VideoQualityPreset("480p", 480, "1500k", "192k"));
            }

            presets.Add(new VideoQualityPreset("360p", 360, "800k", "128k"));

            return presets;
        }

        private async Task RunFFmpegCommandAsync(string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "ffmpeg", // Đảm bảo FFmpeg đã được cài đặt
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    _logger.LogError($"FFmpeg error: {error}");
                    throw new Exception($"FFmpeg processing failed: {error}");
                }
            }
        }

        public async Task<IActionResult> StreamVideoAsync(
            string videoFileName,
            string quality,
            HttpContext httpContext)
        {
            try
            {
                // Tìm file video
                string videoDirectory = Path.Combine(_videoStoragePath,
                    Path.GetFileNameWithoutExtension(videoFileName));
                string videoPath = Path.Combine(videoDirectory, $"video_{quality}.mp4");

                if (!System.IO.File.Exists(videoPath))
                    return new NotFoundResult();

                // Đọc thông tin file
                var fileInfo = new FileInfo(videoPath);
                long fileSize = fileInfo.Length;

                // Hỗ trợ partial content (streaming)
                var request = httpContext.Request;
                var response = httpContext.Response;

                response.Headers.Add("Content-Type", "video/mp4");
                response.Headers.Add("Accept-Ranges", "bytes");
                response.Headers.Add("Content-Length", fileSize.ToString());

                // Xử lý range request
                if (request.Headers.ContainsKey("Range"))
                {
                    var range = request.Headers["Range"].ToString()
                        .Replace("bytes=", "")
                        .Split('-');

                    long start = Convert.ToInt64(range[0]);
                    long end = range.Length > 1 ?
                        Convert.ToInt64(range[1]) :
                        fileSize - 1;

                    long length = end - start + 1;

                    response.StatusCode = 206; // Partial Content
                    response.Headers.Add("Content-Range",
                        $"bytes {start}-{end}/{fileSize}");
                    response.Headers.Add("Content-Length", length.ToString());

                    using (var fileStream = new FileStream(videoPath, FileMode.Open, FileAccess.Read))
                    {
                        fileStream.Seek(start, SeekOrigin.Begin);
                        byte[] buffer = new byte[length];
                        await fileStream.ReadAsync(buffer, 0, (int)length);
                        await response.Body.WriteAsync(buffer);
                    }
                }
                else
                {
                    // Trả về toàn bộ file nếu không có range
                    await httpContext.Response.SendFileAsync(videoPath);
                }

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Video streaming error: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public VideoMetadataDto GetVideoMetadata(string videoFileName)
        {
            try
            {
                string videoDirectory = Path.Combine(_videoStoragePath,
                    Path.GetFileNameWithoutExtension(videoFileName));

                return new VideoMetadataDto
                {
                    Thumbnail = Path.Combine(videoDirectory, "thumbnail.jpg"),
                    AvailableQualities = new[] { "360p", "480p", "720p" }
                };
            }
            catch
            {
                return null;
            }
        }

        
    }
}
