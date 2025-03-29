namespace CourseManagementService.Models
{
    public class VideoQualityPreset
    {
        public string Resolution { get; }
        public int OutputHeight { get; }
        public string BitrateVideo { get; }
        public string BitrateAudio { get; }

        public VideoQualityPreset(string resolution, int outputHeight,
            string bitrateVideo, string bitrateAudio)
        {
            Resolution = resolution;
            OutputHeight = outputHeight;
            BitrateVideo = bitrateVideo;
            BitrateAudio = bitrateAudio;
        }
    }
}
