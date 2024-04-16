namespace WebApplication1.Models
{
    public class CVData
    {
		public string Position { get; set; }
		public string Maintech { get; set; }
		public string Summary { get; set; }
		public string Takumisummary { get; set; }
		public List<string> Takumibulletpoints { get; set; }
		public string Esshwasummary { get; set; }
		public List<string> Esshwabulletpoints { get; set; }
		public string Ewingsummary { get; set; }
		public List<string> Ewingbulletpoints { get; set; }
		public List<string> Hardskillset { get; set; }
		public List<string> Softskillset { get; set; }
	}
}
