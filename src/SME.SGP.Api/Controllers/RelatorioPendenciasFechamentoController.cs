using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios/fechamentos/pendencias")]
    public class RelatorioPendenciasFechamentoController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroRelatorioPendenciasFechamentoDto filtroRelatorioPendenciasFechamentoDto, [FromServices] IRelatorioPendenciasFechamentoUseCase relatorioPendenciasFechamentoUseCase)
        {
            return Ok(await relatorioPendenciasFechamentoUseCase.Executar(filtroRelatorioPendenciasFechamentoDto));
        }

        [HttpGet]
        [Route("tipos")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.PAEE_C, Policy = "Bearer")]
        public IActionResult ObterTipoPendencias([FromQuery] bool opcaoTodos)
        {
            var listaTipo = new List<FiltroBimestreDto>();

            if (opcaoTodos)
            {
                var tipos = new FiltroBimestreDto();
                tipos.Valor = (int)TipoPendenciaGrupo.Todos;
                tipos.Descricao = TipoPendenciaGrupo.Todos.ObterNome();
                listaTipo.Add(tipos);
            }
            var calendario = new FiltroBimestreDto()
            {
                Valor = (int)TipoPendenciaGrupo.Calendario,
                Descricao = TipoPendenciaGrupo.Calendario.ObterNome()
            };
            listaTipo.Add(calendario);

            var diarioClasse = new FiltroBimestreDto()
            {
                Valor = (int)TipoPendenciaGrupo.DiarioClasse,
                Descricao = TipoPendenciaGrupo.DiarioClasse.ObterNome()
            };
            listaTipo.Add(diarioClasse);

            var fechamento = new FiltroBimestreDto()
            {
                Valor = (int)TipoPendenciaGrupo.Fechamento,
                Descricao = TipoPendenciaGrupo.Fechamento.ObterNome()
            };
            listaTipo.Add(fechamento);

            var aee = new FiltroBimestreDto()
            {
                Valor = (int)TipoPendenciaGrupo.AEE,
                Descricao = TipoPendenciaGrupo.AEE.ObterNome()
            };
            listaTipo.Add(aee);

            return Ok(listaTipo);
        }
    }
}