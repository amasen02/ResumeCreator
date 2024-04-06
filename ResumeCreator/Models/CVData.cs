namespace WebApplication1.Models
{
    public class CVData
    {
        public string Position { get; set; }
        public string Maintech { get; set; }
        public string Summary { get; set; }
        public string Takumisummary { get; set; }
        public List<string> Takumibulletpoints { get; set; }
        public string Esshvasummary { get; set; }
        public List<string> Esshvabulletpoints { get; set; }
        public string Ewingssummary { get; set; }
        public List<string> Ewingsbulletpoints { get; set; }
        public List<string> Hardskillset { get; set; }
        public List<string> Softskillset { get; set; }
    }
}
