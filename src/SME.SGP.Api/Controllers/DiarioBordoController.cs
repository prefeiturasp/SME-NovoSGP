using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/diarios-bordo")]
    [Authorize("Bearer")]
    public class DiarioBordoController : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> Obter([FromServices] IObterDiarioBordoUseCase useCase, long id)
        {
            return await  Task.FromResult(Ok(new DiarioBordoDto()
            {
                Auditoria = new AuditoriaDto()
                {
                    AlteradoEm = DateTime.Now,
                    AlteradoPor = "João",
                    AlteradoRF = "794563",
                    CriadoEm = DateTime.Now.AddDays(-1),
                    CriadoPor = "Maria",
                    CriadoRF = "7985698",
                    Id = id
                },
                Aula = new AulaDto()
                {
                    Id = 1,
                    DataAula = DateTime.Now.AddDays(-2),
                    DisciplinaId = "512",
                    AulaCJ = false,
                    DisciplinaNome = "Regência Infantil",
                    Quantidade = 1,
                    RecorrenciaAula = Dominio.RecorrenciaAula.AulaUnica,
                    TipoAula = Dominio.TipoAula.Normal,
                    TipoCalendarioId = 1,
                    TurmaId = "125896321",
                    UeId = "1258963"
                },
                AulaId = 1,
                Data = new DateTime(2021, 01, 01),
                DevolutivaId = 3,
                Devolutivas = "<p>Acessar o sistema com o usuário de CP RF: 8279250&gt; &lt;Acessar o menu&gt; &lt;Diário de Classe&gt; &lt;Devolutivas&gt; &lt;Selecionar um período no calendário de acordo com o período que foi inserida a devolutiva &gt; Então deverá ser apresentado a data em que o CP realizou o cadastro da devolutiva.&nbsp;</p>",
                Observacoes = new List<ObservacaoNotificacoesDiarioBordoDto>()
                {
                     new ObservacaoNotificacoesDiarioBordoDto()
                     {
                          Auditoria =  new AuditoriaDto()
                          {
                            AlteradoEm = DateTime.Now,
                            AlteradoPor = "João",
                            AlteradoRF = "794563",
                            CriadoEm = DateTime.Now.AddDays(-1),
                            CriadoPor = "Maria",
                            CriadoRF = "7985698",
                            Id = id
                          },
                           Observacao = "Acessar o sistema com o usuário de Professor de Ed. Infantil RF: 7226501 > Acessar o menu > Diário de Classe > Diário de bordo > Então deve exibir uma nova seção para inclusão de observações",
                          QtdUsuariosNotificacao = 2
                     },
                     new ObservacaoNotificacoesDiarioBordoDto()
                     {
                          Auditoria =  new AuditoriaDto()
                          {
                            AlteradoEm = DateTime.Now,
                            AlteradoPor = "João",
                            AlteradoRF = "794563",
                            CriadoEm = DateTime.Now.AddDays(-1),
                            CriadoPor = "Maria",
                            CriadoRF = "7985698",
                            Id = id
                          },
                           Observacao = "CT001 – Validar as regras do campo <Observação> dentro do <Diário de Bordo> q edição de campo (ALTERAÇÃO DE TEXTO)",
                          QtdUsuariosNotificacao = 3
                     },

                },
                Planejamento = "Planejamento",
                ReflexoesReplanejamento = "Refelxoes"
            }));
        }

        [HttpGet("detalhes/{id}")]
        [ProducesResponseType(typeof(DiarioBordoDetalhesDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorId([FromServices] IObterDiarioBordoPorIdUseCase useCase, long id)
        {
            return Ok(await useCase.Executar(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_I, Policy = "Bearer")]
        public async Task<IActionResult> Salvar([FromServices] IInserirDiarioBordoUseCase useCase, [FromBody] InserirDiarioBordoDto diarioBordoDto)
        {
            return Ok(await useCase.Executar(diarioBordoDto));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromServices] IAlterarDiarioBordoUseCase useCase, [FromBody] AlterarDiarioBordoDto diarioBordoDto)
        {
            return Ok(await useCase.Executar(diarioBordoDto));
        }

        [HttpGet("devolutivas/{devolutivaId}")]
        [ProducesResponseType(typeof(DiarioBordoDto), 200)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorDevolutiva([FromServices] IObterDiariosBordoPorDevolutiva useCase, long devolutivaId)
        {
            return Ok(await useCase.Executar(devolutivaId));
        }

        [HttpGet("{diarioBordoId}/observacoes")]
        [ProducesResponseType(typeof(IEnumerable<ListarObservacaoDiarioBordoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarObservacoes(long diarioBordoId, [FromServices] IListarObservacaoDiarioBordoUseCase listarObservacaoDiarioBordoUseCase)
        {
            return Ok(await listarObservacaoDiarioBordoUseCase.Executar(diarioBordoId));
        }

        [HttpGet("turmas/{turmaCodigo}/componentes-curriculares/{componenteCurricularId}/inicio/{dataInicio}/fim/{dataFim}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<DiarioBordoDevolutivaDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPorIntervalo([FromServices] IObterDiariosDeBordoPorPeriodoUseCase useCase, string turmaCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            return Ok(await useCase.Executar(new FiltroTurmaComponentePeriodoDto(turmaCodigo, componenteCurricularId, dataInicio, dataFim)));
        }

        [HttpPost("{diarioBordoId}/observacoes")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> AdicionarObservacao(long diarioBordoId, [FromBody] ObservacaoDiarioBordoDto dto, [FromServices] IAdicionarObservacaoDiarioBordoUseCase adicionarObservacaoDiarioBordoUseCase)
        {
            return Ok(await adicionarObservacaoDiarioBordoUseCase.Executar(dto.Observacao, diarioBordoId, dto.UsuariosIdNotificacao));
        }

        [HttpPut("observacoes/{observacaoId}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> AlterarrObservacao(long observacaoId, [FromBody] ObservacaoDiarioBordoDto dto, [FromServices] IAlterarObservacaoDiarioBordoUseCase alterarObservacaoDiarioBordoUseCase)
        {
            return Ok(await alterarObservacaoDiarioBordoUseCase.Executar(dto.Observacao, observacaoId, dto.UsuariosIdNotificacao));
        }

        [HttpDelete("observacoes/{observacaoId}")]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirObservacao(long observacaoId, [FromServices] IExcluirObservacaoDiarioBordoUseCase excluirObservacaoDiarioBordoUseCase)
        {
            return Ok(await excluirObservacaoDiarioBordoUseCase.Executar(observacaoId));
        }

        [HttpGet("titulos/turmas/{turmaId}/componentes-curriculares/{componenteCurricularId}")]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<DiarioBordoTituloDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterTitulosPorIntervalo([FromServices] IObterListagemDiariosDeBordoPorPeriodoUseCase useCase, long turmaId, long componenteCurricularId, DateTime? dataInicio, DateTime? dataFim)
        {
            return Ok(await useCase.Executar(new FiltroListagemDiarioBordoDto(turmaId, componenteCurricularId, dataInicio, dataFim)));
        }

        [HttpGet("notificacoes/usuarios")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.DDB_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUsuariosParaNotificar([FromQuery] ObterUsuarioNotificarDiarioBordoObservacaoDto dto, [FromServices] IObterUsuarioNotificarDiarioBordoObservacaoUseCase obterUsuarioNotificarDiarioBordoObservacaoUseCase)
        {
            return Ok(await obterUsuarioNotificarDiarioBordoObservacaoUseCase.Executar(dto));
        }
    }
}