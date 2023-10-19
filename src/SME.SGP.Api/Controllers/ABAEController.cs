
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/abae")]
    [ValidaDto]
    // [Authorize("Bearer")]
    public class ABAEController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(AuditoriaConselhoClasseAlunoDto), 200)]
        // [Permissao(Permissao.ABA_I, Policy = "Bearer")]
        public async Task<IActionResult> Incluir([FromBody] CadastroAcessoABAEDto cadastroAcessoAbaeDto, [FromServices] ISalvarCadastroAcessoABAEUseCase salvarCadastroAcessoAbaeUse)
        {
            return Ok(salvarCadastroAcessoAbaeUse.Executar(cadastroAcessoAbaeDto));
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(AuditoriaConselhoClasseAlunoDto), 200)]
        // [Permissao(Permissao.ABA_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody] CadastroAcessoABAEDto cadastroAcessoAbaeDto, [FromServices] ISalvarCadastroAcessoABAEUseCase salvarCadastroAcessoAbaeUse)
        {
            return Ok(salvarCadastroAcessoAbaeUse.Executar(cadastroAcessoAbaeDto));
        }
    }
}