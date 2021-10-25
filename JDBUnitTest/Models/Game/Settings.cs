namespace JDBUnitTest.Models.Game
{
    internal class Settings
    {
        public int _id { get; set; }
        public int QualitySchader { get; set; }
        public int QualityVideo { get; set; }
        public int VideoSize { get; set; }
        public Settings() { }
        public Settings(int id)
            :this(id, qualitySchader:100,qualityVideo:100,videoSize:4096)
        {}
        public Settings(int id, int qualitySchader, int qualityVideo, int videoSize)
        {
            _id = id;
            QualitySchader = qualitySchader;
            QualityVideo = qualityVideo;
            VideoSize = videoSize;
        }
    }
}
