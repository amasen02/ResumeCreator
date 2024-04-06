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
 
        string templateUrl = _resumeService.ProcessJsonAndGenerateTemplate(data);
 
        return Ok(templateUrl);
        
    }
}