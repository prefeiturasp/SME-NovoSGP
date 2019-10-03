using Microsoft.AspNetCore.Mvc;
using SME.SGP.Dto;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuarioController : ControllerBase
    {
        [Route("imagens/perfil")]
        [HttpPost]
        public IActionResult ModificarImagem([FromBody]ImagemPerfilDto imagemPerfilDto)
        {
            return Ok("https://telegramic.org/media/avatars/stickers/52cae315e8a464eb80a3.png");
        }
    }
}