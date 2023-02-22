using System;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;
using System.Threading;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaFechamentoUseCase : AbstractUseCase, IPendenciaAulaFechamentoUseCase
    {
        public PendenciaAulaFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<DreUeDto>();
                List<Aula> aulasComPendenciaDiarioClasse = new List<Aula>();

                var aulas = (await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.PlanoAula,
                    "plano_aula",
                    new long[] { (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio },
                    filtro.DreId, filtro.UeId)));
                if (aulas != null)
                    aulasComPendenciaDiarioClasse.AddRange(aulas);

                aulas = await mediator.Send(new ObterPendenciasAtividadeAvaliativaQuery(filtro.DreId, filtro.UeId));
                if (aulas != null)
                    aulasComPendenciaDiarioClasse.AddRange(aulas);

                aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.Frequencia,
                                                                                 "registro_frequencia",
                                                                                 new long[] { (int)Modalidade.EducacaoInfantil, (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio },
                                                                                 filtro.DreId, filtro.UeId));
                if (aulas != null)
                    aulasComPendenciaDiarioClasse.AddRange(aulas);

                var agrupamentoTurmaDisciplina = aulas.GroupBy(aula => new { TurmaCodigo = aula.TurmaId, aula.DisciplinaId, TurmaId = aula.Turma.Id, ModalidadeTipoCalendario = aula.Turma.ModalidadeTipoCalendario });
                foreach (var turmaDisciplina in agrupamentoTurmaDisciplina)
                {
                    var periodoEscolarFechamentoEmAberto = (await mediator.Send(new ObterPeriodoEscolarFechamentoEmAbertoQuery(turmaDisciplina.Key.TurmaCodigo, turmaDisciplina.Key.ModalidadeTipoCalendario, DateTimeExtension.HorarioBrasilia().Date)));
                    if (periodoEscolarFechamentoEmAberto != null)
                    {
                        var situacao = await mediator.Send(new ObterSituacaoFechamentoTurmaComponenteQuery(turmaDisciplina.Key.TurmaId, long.Parse(turmaDisciplina.Key.DisciplinaId), periodoEscolarFechamentoEmAberto.Id));
                        if (situacao == SituacaoFechamento.ProcessadoComSucesso || situacao == SituacaoFechamento.ProcessadoComPendencias)
                        {
                            var fechamentosTurmaDisciplina = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinaBimestreQuery(turmaDisciplina.Key.TurmaCodigo, 
                                                                                                                                                     long.Parse(turmaDisciplina.Key.DisciplinaId), 
                                                                                                                                                     periodoEscolarFechamentoEmAberto.Bimestre));
                            if (fechamentosTurmaDisciplina.Any())
                            {
                                var fechamentoTurmaDisciplina = fechamentosTurmaDisciplina.FirstOrDefault();
                                var disciplinasEol = await mediator.Send(new ObterDisciplinasPorIdsQuery(new[] { fechamentoTurmaDisciplina.DisciplinaId }));

                                var disciplina = disciplinasEol is null
                                    ? throw new NegocioException("Não foi possível localizar o componente curricular no EOL.")
                                    : disciplinasEol.FirstOrDefault();

                                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaDisciplina.Key.TurmaCodigo));
                                var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdQuery(fechamentoTurmaDisciplina.FechamentoTurmaId));

                                await GerarPendenciasFechamento(fechamentoTurmaDisciplina.DisciplinaId,
                                    turma.CodigoTurma,
                                    turma.Nome,
                                    periodoEscolarFechamentoEmAberto.PeriodoInicio,
                                    periodoEscolarFechamentoEmAberto.PeriodoFim,
                                    periodoEscolarFechamentoEmAberto.Bimestre,
                                    fechamentoTurmaDisciplina.Id,
                                    fechamentoTurmaDisciplina.Justificativa,
                                    fechamentoTurmaDisciplina.CriadoRF,
                                    fechamentoTurmaDisciplina.FechamentoTurma.TurmaId,
                                    true,
                                    disciplina.RegistraFrequencia);
                            }
                        }
                    } 
                     

                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao publicar mensagem para geração de pendências fechamento de aulas/diários de classe com pendência.",  LogNivel.Critico, LogContexto.Fechamento, ex.Message,innerException: ex.InnerException.ToString(),rastreamento:ex.StackTrace));
                throw;
            }
        }

        private async Task GerarPendenciasFechamento(long componenteCurricularId, string turmaCodigo, string turmaNome, DateTime periodoEscolarInicio, DateTime periodoEscolarFim, int bimestre, long fechamentoTurmaDisciplinaId, string justificativa, string criadoRF, long turmaId, bool componenteSemNota = false, bool registraFrequencia = true)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            await mediator.Send(new IncluirFilaGeracaoPendenciasFechamentoCommand(
                componenteCurricularId,
                turmaCodigo,
                turmaNome,
                periodoEscolarInicio,
                periodoEscolarFim,
                bimestre,
                usuarioLogado,
                fechamentoTurmaDisciplinaId,
                justificativa,
                criadoRF,
                turmaId,
                componenteSemNota,
                registraFrequencia));
        }
    }
}
