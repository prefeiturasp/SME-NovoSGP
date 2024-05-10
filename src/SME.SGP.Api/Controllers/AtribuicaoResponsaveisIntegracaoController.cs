using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Middlewares;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicoes/responsaveis/integracoes")]
    [ChaveIntegracaoSgpApi]
    public class AtribuicaoResponsaveisIntegracaoController : ControllerBase
    {
        [HttpGet("escola/{ueCodigo}/tipo/{tipo}")]
        [ProducesResponseType(typeof(IEnumerable<FuncionarioDTO>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> Get(string ueCodigo,int tipo, [FromServices] IListarAtribuicoesResponsaveisPorFiltroUseCase useCase)
        {
            return Ok(await useCase.Executar(new AtribuicaoResponsaveisFiltroDto{UeCodigo = ueCodigo,Tipo = (TipoResponsavelAtribuicao)tipo}));
        }

    }
}