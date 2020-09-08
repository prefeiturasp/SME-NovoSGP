using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Pkcs;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("file/upload")]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Editor([FromForm] IFormFile file, [FromForm] string source, [FromForm] string path)
        {




            var filess = Request.Form.Files;
            // Get the file from the POST request
            var theFile = filess.FirstOrDefault();
            
            var nomeArquivo = $"{Guid.NewGuid()}-{theFile.FileName}";
            var caminhoArquivo = Path.Combine("Imagens", nomeArquivo);

            //// Get the server path, wwwroot
            //string webRootPath = _hostingEnvironment.WebRootPath;
            //var fileRoute = Path.Combine(webRootPath, "uploads");

            //string fullPath = Path.Combine(fileRoute, theFile.FileName);

            // Create directory if it does not exist.
            FileInfo dir = new FileInfo(caminhoArquivo);
            if (!dir.Exists)
            {
                dir.Directory.Create();
            }
            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                await theFile.CopyToAsync(stream);
            }

            string AppBaseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            return Ok(new
            {
                success = true,
                data = new
                {
                    files = new[] { nomeArquivo },
                    baseurl = $"{AppBaseUrl}/imagens/",
                    message = "",
                    error = "",
                    path = $"{AppBaseUrl}/imagens/{nomeArquivo}"
                }
            });
        }
    }
}