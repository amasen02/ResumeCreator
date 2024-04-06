using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

using static System.Collections.Specialized.BitVector32;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Xml.Linq;
using GroupDocs.Conversion;
using GroupDocs.Conversion.Options.Convert;
using WebApplication1.Models;
using WebApplication1.Services;
using System.Security;


[ApiController]
[Route("api/cv")]
public class CVController : ControllerBase
{
    private readonly ResumeService _resumeService;

    public CVController(ResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    [HttpPost]
    public ActionResult GeneratePdf([FromBody] CVData data)
    {
        try
        {

            string templateUrl = _resumeService.ProcessJsonAndGenerateTemplate(data);
            if (!System.IO.File.Exists(templateUrl))
                return NotFound();

            var fileStream = new FileStream(templateUrl, FileMode.Open, FileAccess.Read);
            return File(fileStream, "text/plain", "output.tex");
      

        }
        catch (Exception ex)
        {

            return StatusCode(500, ex.Message);
        }
        
    }
}