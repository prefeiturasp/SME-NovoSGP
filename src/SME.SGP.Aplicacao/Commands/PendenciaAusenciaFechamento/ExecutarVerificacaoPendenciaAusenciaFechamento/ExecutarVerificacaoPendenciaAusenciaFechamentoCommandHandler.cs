using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAusenciaFechamentoCommandHandler : IRequestHandler<ExecutarVerificacaoPendenciaAusenciaFechamentoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEol;
        public ExecutarVerificacaoPendenciaAusenciaFechamentoCommandHandler(IMediator mediator, IServicoEol servicoEol)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));

        }

        public async Task<bool> Handle(ExecutarVerificacaoPendenciaAusenciaFechamentoCommand request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasPorAnoModalidadeQuery(DateTime.Now.Year, request.ModalidadeTipoCalendario.ObterModalidadesTurma()));

            var periodoFechamentoBimestres = await mediator.Send(new ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery((int)request.ModalidadeTipoCalendario, DateTime.Now.AddDays(request.DiasParaGeracaoDePendencia)));
            var componentes = await mediator.Send(new ObterComponentesCurricularesQuery());
            foreach (var turma in turmas.Where(t => periodoFechamentoBimestres.Any(p => t.UeId == p.PeriodoFechamento.UeId.Value)))
            {
                var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turma.CodigoTurma));
                foreach (var periodoFechamentoBimestre in periodoFechamentoBimestres)
                {
                    var professoresTurma = await servicoEol.ObterProfessoresTitularesDisciplinas(turma.CodigoTurma);
                    foreach (var professorTurma in professoresTurma)
                    {
                        var obterComponenteCurricular = componentes.FirstOrDefault(c => long.Parse(c.Codigo) == professorTurma.DisciplinaId);
                        if (obterComponenteCurricular != null)
                        {
                            if (professorTurma.ProfessorRf != "")
                            {
                                var verificaFechamento = await mediator.Send(new VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery(turma.Id, long.Parse(obterComponenteCurricular.Codigo), periodoFechamentoBimestre.PeriodoEscolar.Id));

                                if (!verificaFechamento)
                                {
                                    if (!await ExistePendenciaProfessor(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, periodoFechamentoBimestre.PeriodoEscolar.Id))
                                        await IncluirPendenciaAusenciaFechamento(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, periodoFechamentoBimestre.PeriodoEscolar.Bimestre, obterComponenteCurricular.Descricao, obterComponenteCurricular.LancaNota, periodoFechamentoBimestre.PeriodoEscolar.Id);
                                }

                                if(EhBimestreFinal(request.ModalidadeTipoCalendario, periodoFechamentoBimestre.PeriodoEscolar.Bimestre))
                                {
                                    var verificaFechamentoFinal = await mediator.Send(new VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery(turma.Id, long.Parse(obterComponenteCurricular.Codigo), null));
                                    if (!verificaFechamentoFinal)
                                    {

                                        if (!await ExistePendenciaProfessor(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, null))
                                            await IncluirPendenciaAusenciaFechamento(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, periodoFechamentoBimestre.PeriodoEscolar.Bimestre, obterComponenteCurricular.Descricao, obterComponenteCurricular.LancaNota, null);
                                    }
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
            return (bimestre == 2 && modalidadeTipoCalendario == ModalidadeTipoCalendario.EJA) || bimestre == 4;
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

            var escolaUe = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) - Turma {turma.Nome}";
            var titulo = $"Processo de fechamento não iniciado de {componenteCurricularNome} no {bimestre}º bimestre - <i>{escolaUe}</i>";

            var descricao = $"<i>O componente curricular <b>{componenteCurricularNome}</b> ainda não teve o processo de fechamento do <b>{bimestre}º bimestre</b> iniciado na <b>{escolaUe}</b></i>";

            var instrucao = "";
            if (lancaNota)
            {
                instrucao = "Você precisa acessar a tela de Notas e Conceitos e registrar a avaliação final dos estudantes para que o processo seja iniciado";
            }
            else
            {
                instrucao = "Você precisa acessar a tela de Fechamento do Bimestre e executar o processo de fechamento.";
            }


            await mediator.Send(new SalvarPendenciaAusenciaFechamentoCommand(turma.Id, componenteCurricularId, professorRf, titulo, descricao, instrucao, periodoEscolarId));

        }

        private async Task<DisciplinaDto> ObterComponenteCurricular(long componenteCurricularId)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new[] { componenteCurricularId }));
            return componentes.FirstOrDefault();
        }

    }
}
