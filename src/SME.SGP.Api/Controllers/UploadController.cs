using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/uploads")]
    //[Authorize("Bearer")]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<AbrangenciaDreRetorno>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Editor([FromForm] IFormFile upload)
        {

            var caminhoBase = AppDomain.CurrentDomain.BaseDirectory;
            var nomeArquivo = Path.Combine("Imagens", upload.FileName);


            using (var stream = System.IO.File.Create(nomeArquivo))
            {
                await upload.CopyToAsync(stream);
            }

            return Ok(new
            {
                url = $"https://localhost:5001/imagens/{upload.FileName}"
            });
        }
    }
}