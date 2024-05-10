using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/professores/aulas")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class AulaController : ControllerBase
    {
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody] PersistirAulaDto dto, long id, [FromServices] IAlterarAulaUseCase alterarAulaUseCase)
        {
            return Ok(await alterarAulaUseCase.Executar(dto));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AulaConsultaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPorId(long id, [FromServices] IObterAulaPorIdUseCase obterAulaPorIdUseCase)
        {
            return Ok(await obterAulaPorIdUseCase.Executar(id));
        }

        [HttpDelete("{id}/recorrencias/{recorrencia}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir(long id, RecorrenciaAula recorrencia, [FromServices] IExcluirAulaUseCase excluirAulaUseCase)
        {
            return Ok(await excluirAulaUseCase.Executar(new ExcluirAulaDto()
            {
                AulaId = id,
                RecorrenciaAula = recorrencia                
            }));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> Inserir([FromBody] PersistirAulaDto inserirAulaDto, [FromServices] IInserirAulaUseCase inserirAulaUseCase)
        {
            return Ok(await inserirAulaUseCase.Executar(inserirAulaDto));
        }

        [HttpGet("{aulaId}/recorrencias/serie/{recorrenciaSelecionada}")]
        [ProducesResponseType(typeof(AulaRecorrenciaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CP_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRecorrenciaDaSerie(long aulaId, [FromServices] IConsultasAula consultas, [FromServices] IObterFrequenciaOuPlanoNaRecorrenciaUseCase obterFrequenciaOuPlanoNaRecorrenciaUseCase)
        {
            var recorrencia = consultas.ObterRecorrenciaDaSerie(aulaId);
            
            var quantidadeAulas = recorrencia == (int)RecorrenciaAula.AulaUnica 
                                               ? 1
                                               : await consultas
                                                       .ObterQuantidadeAulasRecorrentes(aulaId, 
                                                                                        RecorrenciaAula.RepetirTodosBimestres);
                
            var existeFrequenciaPlanoAula = await obterFrequenciaOuPlanoNaRecorrenciaUseCase.Executar(aulaId);

            var retorno = new AulaRecorrenciaDto()
            {
                AulaId = aulaId,
                RecorrenciaAula = recorrencia,
                QuantidadeAulasRecorrentes = quantidadeAulas,
                ExisteFrequenciaOuPlanoAula = existeFrequenciaPlanoAula
            };

            return Ok(retorno);
        }

        [HttpGet("{aulaId}/turmas/{turmaCodigo}/componente-curricular/{componenteCurricular}")]
        [ProducesResponseType(typeof(CadastroAulaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> CadastroAula([FromServices] IPodeCadastrarAulaUseCase podeCadastrarAulaUseCase, long aulaId, string turmaCodigo, long componenteCurricular, [FromQuery] DateTime dataAula, [FromQuery] bool ehRegencia, [FromQuery] TipoAula tipoAula)
        {
            return Ok(await podeCadastrarAulaUseCase.Executar(new FiltroPodeCadastrarAulaDto(aulaId, turmaCodigo, new long[] { componenteCurricular }, dataAula, ehRegencia, tipoAula)));
        }

        [HttpGet("sincronizar/aulas-regencia")]
        [ProducesResponseType(typeof(CadastroAulaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_A, Policy = "Bearer")]
        public async Task<IActionResult> SincronizarAulasRegencia([FromQuery] long? codigoTurma, [FromServices] IMediator mediator)
        {
            var dados = new DadosCriacaoAulasAutomaticasCarregamentoDto() { CodigoTurma = codigoTurma?.ToString() };
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.CarregarDadosUeTurmaRegenciaAutomaticamente, dados, Guid.NewGuid(), null));
            return Ok();
        }
    }
}