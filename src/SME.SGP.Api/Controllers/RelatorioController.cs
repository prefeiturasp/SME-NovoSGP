using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/relatorios")]
    [Authorize("Bearer")]
    public class RelatorioController : ControllerBase
    {
        [HttpGet("{codigoCorrelacao}")]
        public async Task<IActionResult> Download(Guid codigoCorrelacao, [FromServices] IReceberDadosDownloadRelatorioUseCase downloadRelatorioUseCase, [FromServices] ISevicoJasper servicoJasper)
        {
            var (relatorio, contentType, nomeArquivo) = await downloadRelatorioUseCase.Executar(codigoCorrelacao);

            return File(relatorio, contentType, nomeArquivo);
        }
        
        [HttpPost("conselhos-classe/atas-finais")]
        public async Task<IActionResult> ConselhoClasseAtaFinal([FromBody]FiltroRelatorioConselhoClasseAtaFinalDto filtroRelatorioConselhoClasseAtaFinalDto, [FromServices] IRelatorioConselhoClasseAtaFinalUseCase relatorioConselhoClasseAtaFinalUseCase)
        {
            return Ok(await relatorioConselhoClasseAtaFinalUseCase.Executar(filtroRelatorioConselhoClasseAtaFinalDto));
        }
     
        [HttpPost("faltas-frequencia")]
        public async Task<IActionResult> FaltasFrequencia([FromBody] FiltroRelatorioFaltasFrequenciaDto filtroRelatorioFaltasFrequenciaDto, [FromServices] IGerarRelatorioFaltasFrequenciaUseCase gerarRelatorioFaltasFrequenciaUseCase)
        {
            return Ok(await gerarRelatorioFaltasFrequenciaUseCase.Executar(filtroRelatorioFaltasFrequenciaDto));
        }

        [HttpPost("calendarios/impressao")]
        public async Task<IActionResult> Calendario([FromBody] FiltroRelatorioCalendarioDto filtroRelatorioCalendarioDto, [FromServices] IRelatorioCalendarioUseCase relatorioCalendarioUseCase)
        {
            return Ok(await relatorioCalendarioUseCase.Executar(filtroRelatorioCalendarioDto));
        }

        [HttpPost("resumopap/impressao")]
        public async Task<IActionResult> ResumoPAP([FromBody] FiltroRelatorioResumoPAPDto filtroRelatorioResumoPAPDto, [FromServices] IRelatorioResumoPAPUseCase relatorioResumoPAPUseCase)
        {
            return Ok(await relatorioResumoPAPUseCase.Executar(filtroRelatorioResumoPAPDto));
        }

        [HttpPost("graficopap/impressao")]
        public async Task<IActionResult> GraficoPAP([FromBody] FiltroRelatorioResumoPAPDto filtroRelatorioGraficoPAPDto, [FromServices] IRelatorioGraficoPAPUseCase relatorioGraficoPAPUseCase)
        {
            return Ok(await relatorioGraficoPAPUseCase.Executar(filtroRelatorioGraficoPAPDto));
        }

        [HttpPost("plano-aula")]
        public async Task<IActionResult> PlanoAula([FromBody] FiltroRelatorioPlanoAulaDto filtro, [FromServices] IRelatorioPlanoAulaUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPost("controle-grade/impressao")]
        public async Task<IActionResult> ControleGrade([FromBody] FiltroRelatorioControleGrade filtro, [FromServices] IRelatorioControleGradeUseCase relatorioUseCase)
        {
            return Ok(await relatorioUseCase.Executar(filtro));
        }

        
        [HttpPost("notificacoes/impressao")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.RDN_C, Policy = "Bearer")]
        public async Task<IActionResult> Notificacoes([FromBody] FiltroRelatorioNotificacao filtro, [FromServices] IRelatorioNotificacaoUseCase useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpPost("usuarios/impressao")]
        public async Task<IActionResult> Usuarios([FromBody] FiltroRelatorioUsuarios filtro, [FromServices] IRelatorioUsuariosUseCase relatorioUseCase)
        {
            return Ok(await relatorioUseCase.Executar(filtro));
        }

        [HttpPost("atribuicoes/cjs")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Gerar(FiltroRelatorioAtribuicaoCJDto filtros, [FromServices] IRelatorioAtribuicaoCJUseCase relatorioAtribuicaoCJUseCase)
        {
            return Ok(await relatorioAtribuicaoCJUseCase.Executar(filtros));
        }
        
        [HttpPost("historico-alteracao-notas")]
        public async Task<IActionResult> AlteracaoNotas([FromBody] FiltroRelatorioAlteracaoNotas filtro, [FromServices] IRelatorioAlteracaoNotasUseCase relatorioUseCase)
        {
            if (filtro.ModalidadeTurma == Dominio.Modalidade.Infantil)
                throw new NegocioException("Não é possível gerar este relatório para a modalidade infantil.");
            return Ok(await relatorioUseCase.Executar(filtro));
        }
        [HttpPost("ae/adesao")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AdesaoApp([FromBody] FiltroRelatorioAEAdesaoDto filtro, [FromServices] IRelatorioAEAdesaoUseCase relatorioUseCase)
        {
            return Ok(await relatorioUseCase.Executar(filtro));
        }

        [HttpPost("escola-aqui/dados-leitura")]
        public async Task<IActionResult> LeituraComunicados([FromBody] FiltroRelatorioLeituraComunicados filtro, [FromServices] IRelatorioLeituraComunicadosUseCase relatorioUseCase)
        {            
            return Ok(await relatorioUseCase.Executar(filtro));
        }

        [HttpPost("diario-classe/planejamento-diario")]
        public async Task<IActionResult> PlanejamentoDiario([FromBody] FiltroRelatorioPlanejamentoDiario filtro, [FromServices] IRelatorioPlanejamentoDiarioUseCase relatorioUseCase)
        {
            return Ok(await relatorioUseCase.Executar(filtro));
        }

    }
}