﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAusenciaFechamentoCommandHandler : IRequestHandler<ExecutarVerificacaoPendenciaAusenciaFechamentoCommand, bool>
    {
        private readonly IMediator mediator;
        public ExecutarVerificacaoPendenciaAusenciaFechamentoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutarVerificacaoPendenciaAusenciaFechamentoCommand request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasPorAnoModalidadeQuery(DateTime.Now.Year, request.ModalidadeTipoCalendario.ObterModalidades()));

            var periodoFechamentoBimestres = await mediator.Send(new ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery((int)request.ModalidadeTipoCalendario, DateTime.Now.Date.AddDays(request.DiasParaGeracaoDePendencia)));
            var componentes = await mediator.Send(ObterComponentesCurricularesQuery.Instance);

            foreach (var turma in turmas.Where(t => periodoFechamentoBimestres.Any(p => t.UeId == p.PeriodoFechamento.UeId.Value)))
            {
                var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turma.CodigoTurma));
                foreach (var periodoFechamentoBimestre in periodoFechamentoBimestres)
                {
                    if (periodoFechamentoBimestre.PeriodoEscolar.TipoCalendario.Modalidade.EhEducacaoInfantil())
                        continue;

                    var professoresTurma = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turma.CodigoTurma));
                    foreach (var professorTurma in professoresTurma)
                    {
                        var obterComponenteCurricular = componentes.FirstOrDefault(c => professorTurma.DisciplinasId.Contains(long.Parse(c.Codigo)));
                        if (obterComponenteCurricular.NaoEhNulo())
                        {
                            if (professorTurma.ProfessorRf != "")
                            {
                                try
                                {
                                    var existeFechamento = await mediator.Send(new VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery(turma.Id, long.Parse(obterComponenteCurricular.Codigo), periodoFechamentoBimestre.PeriodoEscolar.Id));

                                    if (!existeFechamento)
                                    {
                                        if (!await ExistePendenciaProfessor(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, periodoFechamentoBimestre.PeriodoEscolar.Id))
                                            await IncluirPendenciaAusenciaFechamento(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, periodoFechamentoBimestre.PeriodoEscolar.Bimestre, obterComponenteCurricular.Descricao, obterComponenteCurricular.LancaNota, periodoFechamentoBimestre.PeriodoEscolar.Id);
                                    }

                                    if (EhBimestreFinal(request.ModalidadeTipoCalendario, periodoFechamentoBimestre.PeriodoEscolar.Bimestre))
                                    {
                                        existeFechamento = await mediator.Send(new VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery(turma.Id, long.Parse(obterComponenteCurricular.Codigo), null));
                                        if (!existeFechamento)
                                        {
                                            if (!await ExistePendenciaProfessor(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, null))
                                                await IncluirPendenciaAusenciaFechamento(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, periodoFechamentoBimestre.PeriodoEscolar.Bimestre, obterComponenteCurricular.Descricao, obterComponenteCurricular.LancaNota, null);
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    await mediator.Send(new SalvarLogViaRabbitCommand($"Erro em Executar Verificacao Pendencia Ausencia Fechamento.", LogNivel.Negocio, LogContexto.Fechamento, ex.Message));
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        private bool EhBimestreFinal(ModalidadeTipoCalendario modalidadeTipoCalendario, int bimestre)
        {
            return (bimestre == 2 && modalidadeTipoCalendario.EhEjaOuCelp()) || bimestre == 4;
        }

        private async Task<bool> ExistePendenciaProfessor(long turmaId, long componenteCurricularId, string professorRf, long? periodoEscolarId)
           => await mediator.Send(new ExistePendenciaProfessorPorTurmaEComponenteQuery(turmaId,
                                                                                       componenteCurricularId,
                                                                                       periodoEscolarId,
                                                                                       professorRf,
                                                                                       TipoPendencia.AusenciaFechamento));

        private async Task IncluirPendenciaAusenciaFechamento(long turmaId, long componenteCurricularId, string professorRf, int bimestre, string componenteCurricularNome, bool lancaNota, long? periodoEscolarId)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

            var descricaoBimestre = periodoEscolarId.HasValue ? $"{bimestre}º bimestre" : "bimestre final";

            var escolaUe = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var titulo = $"Processo de fechamento não iniciado de {componenteCurricularNome} no {descricaoBimestre} - {escolaUe}";

            var descricao = $"<i>O componente curricular <b>{componenteCurricularNome}</b> ainda não teve o processo de fechamento do <b>{descricaoBimestre}</b> iniciado na <b>{escolaUe}</b></i>";

            var instrucao = lancaNota ?
                "Você precisa acessar a tela de Notas e Conceitos e registrar a avaliação final dos estudantes para que o processo seja iniciado" :
                "Você precisa acessar a tela de Fechamento do Bimestre e executar o processo de fechamento.";

            await mediator.Send(new SalvarPendenciaAusenciaFechamentoCommand(turma.Id, componenteCurricularId, professorRf, titulo, descricao, instrucao, periodoEscolarId));
        }

        private async Task<DisciplinaDto> ObterComponenteCurricular(long componenteCurricularId)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new[] { componenteCurricularId }));
            return componentes.FirstOrDefault();
        }

    }
}
