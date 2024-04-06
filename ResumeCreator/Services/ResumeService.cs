using Aspose.TeX;
using GroupDocs.Conversion;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ResumeService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ResumeService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public string ProcessJsonAndGenerateTemplate(CVData data)
        {
            string templateFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "template.tex");
            string outputFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "output.tex");

            // Read the template content from the file
            string template = File.ReadAllText(templateFilePath);


            // Replace placeholders with JSON values
            template = template.Replace("##position##", EscapeSpecialCharacters(data.Position));
            template = template.Replace("##maintech##", ConvertToLaTeX(EscapeSpecialCharacters(data.Maintech)));
            template = template.Replace("##summary##", EscapeSpecialCharacters(data.Summary));
            template = template.Replace("##takmisummary##", EscapeSpecialCharacters(data.Takumisummary));
            template = template.Replace("##essawasummary##", EscapeSpecialCharacters(data.Esshvasummary));
            template = template.Replace("##ewingsummary##", EscapeSpecialCharacters(data.Ewingssummary));

            // Replace bullet points for each experience
            template = ReplaceBulletPoints(template, "##takumibulletpoints##", data.Takumibulletpoints);
            template = ReplaceBulletPoints(template, "##esshvabulletpoints##", data.Esshvabulletpoints);
            template = ReplaceBulletPoints(template, "##ewingbuletpoints##", data.Ewingsbulletpoints);

            // Replace technical skills and soft skills
            template = ReplaceSkills(template, "##hardskillset##", data.Hardskillset);
            template = ReplaceSoftSkills(template, "##softskillset##", data.Softskillset);

            // Write the updated template to a new file
            File.WriteAllText(outputFilePath, template);
            return Path.Combine(_hostingEnvironment.ContentRootPath, "output.tex");

        }


        private string EscapeSpecialCharacters(string input)
        {
            return input.Replace("#", @"\#").Replace("%", @"\%");
        }

        private string ConvertToLaTeX(string input)
        {
            string allCaps = input.ToUpper();
            allCaps = allCaps.Replace("|", $@"{{\textbar{{}}}}");
            string formattedString = $@"\centerline{{\textbf{{\large{{{allCaps}}}}}}}";
            return formattedString;
        }

        private string ReplaceBulletPoints(string template, string placeholder, IEnumerable<string> data)
        {
            StringBuilder bulletPoints = new StringBuilder();
            foreach (string item in data)
            {
                string itemText = EscapeSpecialCharacters(item);
                int colonIndex = itemText.IndexOf(":");
                if (colonIndex != -1)
                {
                    string beforeColon = itemText.Substring(0, colonIndex);
                    string afterColon = itemText.Substring(colonIndex + 1);
                    bulletPoints.Append("\\item[--] ").Append(@"\textbf{").Append(beforeColon).Append("}: ").Append(afterColon);
                }
                else
                {
                    bulletPoints.Append("\\item[--] ").Append(itemText);
                }
                if (item != data.Last())
                {
                    bulletPoints.AppendLine();
                }
            }
            return template.Replace(placeholder, bulletPoints.ToString());
        }

        private string ReplaceSkills(string template, string placeholder, IEnumerable<string> data)
        {
            StringBuilder skills = new StringBuilder();
            foreach (string item in data)
            {
                string itemText = EscapeSpecialCharacters(item);
                int colonIndex = itemText.IndexOf(":");
                if (colonIndex != -1)
                {
                    string beforeColon = itemText.Substring(0, colonIndex);
                    string afterColon = itemText.Substring(colonIndex + 1);
                    skills.Append("\\cvitem {").Append(beforeColon).Append("}{").Append(afterColon).Append("}");
                }
                skills.AppendLine();
            }
            return template.Replace(placeholder, skills.ToString());
        }

        private string ReplaceSoftSkills(string template, string placeholder, IEnumerable<string> data)
        {
            StringBuilder skills = new StringBuilder();
            foreach (string item in data)
            {
                string itemText = EscapeSpecialCharacters(item);
                skills.Append(itemText);
                if (item != data.Last())
                {
                    skills.Append(", ");
                }
            }
            return template.Replace(placeholder, skills.ToString());
        }

    }
}
