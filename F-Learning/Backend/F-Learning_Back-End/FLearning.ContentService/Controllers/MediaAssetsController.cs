using FLearning.ContentService.Data;
using FLearning.ContentService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FLearning.ContentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaAssetsController : ControllerBase
    {
        private readonly ContentDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MediaAssetsController(ContentDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/MediaAssets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaAsset>>> GetAll()
        {
            return await _context.MediaAssets
                .OrderByDescending(m => m.UploadedDate)
                .ToListAsync();
        }

        // GET: api/MediaAssets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaAsset>> GetById(int id)
        {
            var asset = await _context.MediaAssets.FindAsync(id);
            if (asset == null)
                return NotFound();
            return asset;
        }

        // POST: api/MediaAssets/Upload
        // Upload file media qua form-data: key "file" (IFormFile) và optional "uploadedBy", "description"
        [HttpPost("Upload")]
        public async Task<ActionResult<MediaAsset>> Upload([FromForm] IFormFile file, [FromForm] string? uploadedBy, [FromForm] string? description)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Xác định thư mục lưu file: dùng thư mục "wwwroot/media"
            string wwwRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(wwwRootPath))
                wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(wwwRootPath, "media");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Tạo tên file duy nhất
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                // Lưu file vào disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Tạo đối tượng MediaAsset
                var asset = new MediaAsset
                {
                    FileName = file.FileName,
                    FileUrl = "/media/" + uniqueFileName, // URL tương đối dùng với UseStaticFiles
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    UploadedDate = DateTime.UtcNow,
                    UploadedBy = uploadedBy,
                    Description = description
                };

                _context.MediaAssets.Add(asset);
                await _context.SaveChangesAsync();

                // (Tuỳ chọn) Ghi log upload thành công
                //_context.MediaLogs.Add(new MediaLog { MediaAssetId = asset.MediaAssetId, Action = "Upload", LoggedDate = DateTime.UtcNow });
                //await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = asset.MediaAssetId }, asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error uploading file: " + ex.Message);
            }
        }

        // PUT: api/MediaAssets/{id}
        // Cập nhật metadata (không upload lại file)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MediaAsset model)
        {
            if (id != model.MediaAssetId)
                return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MediaAssets.Any(m => m.MediaAssetId == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/MediaAssets/{id}
        // Xoá media asset và xoá file khỏi disk
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _context.MediaAssets.FindAsync(id);
            if (asset == null)
                return NotFound();

            // Xoá file từ disk
            string wwwRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(wwwRootPath))
                wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(wwwRootPath, "media");
            var fileName = Path.GetFileName(asset.FileUrl);
            var filePath = Path.Combine(uploadsFolder, fileName);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _context.MediaAssets.Remove(asset);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/MediaAssets/Download/{id}
        // Tải file media về hoặc trả file để stream
        [HttpGet("Download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var asset = await _context.MediaAssets.FindAsync(id);
            if (asset == null)
                return NotFound();

            string wwwRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(wwwRootPath))
                wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(wwwRootPath, "media");
            var fileName = Path.GetFileName(asset.FileUrl);
            var filePath = Path.Combine(uploadsFolder, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on disk.");

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, asset.ContentType, asset.FileName);
        }

        // GET: api/MediaAssets/Search?query=abc
        // Tìm kiếm media asset theo từ khóa (trong FileName hoặc Description)
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<MediaAsset>>> Search([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query string is empty.");

            var results = await _context.MediaAssets
                .Where(m => m.FileName.Contains(query) || (m.Description != null && m.Description.Contains(query)))
                .OrderByDescending(m => m.UploadedDate)
                .ToListAsync();

            return Ok(results);
        }

        // POST: api/MediaAssets/GenerateThumbnail/{id}
        // (Tuỳ chọn) Tạo thumbnail cho media asset (chỉ áp dụng cho hình ảnh)
        [HttpPost("GenerateThumbnail/{id}")]
        public async Task<IActionResult> GenerateThumbnail(int id)
        {
            var asset = await _context.MediaAssets.FindAsync(id);
            if (asset == null)
                return NotFound();

            if (!asset.ContentType.StartsWith("image/"))
                return BadRequest("Thumbnail generation is only supported for image files.");

            try
            {
                // Ví dụ sử dụng ImageSharp hoặc SkiaSharp để tạo thumbnail
                // Ở đây, ta chỉ mô phỏng việc tạo thumbnail bằng cách copy file gốc và đổi tên
                string wwwRootPath = _env.WebRootPath;
                if (string.IsNullOrEmpty(wwwRootPath))
                    wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsFolder = Path.Combine(wwwRootPath, "media");
                string thumbnailFileName = "thumb_" + Path.GetFileName(asset.FileUrl);
                string thumbnailPath = Path.Combine(uploadsFolder, thumbnailFileName);

                // Giả lập: copy file gốc thành thumbnail (trong thực tế, bạn sẽ resize hình ảnh)
                System.IO.File.Copy(Path.Combine(uploadsFolder, Path.GetFileName(asset.FileUrl)), thumbnailPath, true);

                // Lưu URL thumbnail vào asset
                asset.ThumbnailUrl = "/media/" + thumbnailFileName;
                _context.Entry(asset).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { ThumbnailUrl = asset.ThumbnailUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error generating thumbnail: " + ex.Message);
            }
        }
    }
}
