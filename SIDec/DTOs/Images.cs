namespace SIDec.DTOs
{
    public class Images
    {
        public string Path { get; set; }
        public string Width { get; set; }
        public string Heigth { get; set; }
        public Images(string image)
        {
            Path = image;
        }
    }
}