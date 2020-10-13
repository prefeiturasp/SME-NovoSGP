using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    /// <summary>
    /// Controller de exemplo apenas para Poc e demonstração de upload com o componente de editor de texto Jodit
    /// </summary>
    [ApiController]
    [Route("api/file/upload")]
    [Authorize("Bearer")]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Editor()
        {
            var filess = Request.Form.Files;
            var theFile = filess.FirstOrDefault();

            var nomeArquivo = $"{Guid.NewGuid()}-{theFile.FileName}";
            var caminhoArquivo = Path.Combine("Imagens", nomeArquivo);

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
