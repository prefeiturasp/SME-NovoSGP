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
            var turmas = await mediator.Send(new ObterTurmasPorAnoModalidadeQuery(DateTime.Now.Year, (int)request.Modalidade));

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorModalidadeDataFechamentoQuery((int)request.ModalidadeTipoCalendario, DateTime.Now.AddDays(request.DiasParaGeracaoDePendencia)));
            var componentes = await mediator.Send(new ObterComponentesCurricularesQuery());
            foreach (var turma in turmas)
            {
                var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turma.CodigoTurma));
                foreach (var periodoEscolar in periodosEscolares)
                {
                    var professoresTurma = await servicoEol.ObterProfessoresTitularesDisciplinas(turma.CodigoTurma);
                    foreach (var professorTurma in professoresTurma)
                    {
                        var obterComponenteCurricular = componentes.FirstOrDefault(c => long.Parse(c.Codigo) == professorTurma.DisciplinaId);
                        if (obterComponenteCurricular != null)
                        {
                            var verificaFechamento = await mediator.Send(new VerificaFechamentoTurmaDisciplinaPorIdCCEPeriodoEscolarQuery(turma.Id, long.Parse(obterComponenteCurricular.Codigo), periodoEscolar.Id));
                            if (!verificaFechamento)
                            {
                                if(professorTurma.ProfessorRf != "")
                                {
                                    if (!await ExistePendenciaProfessor(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf))
                                        await IncluirPendenciaAusenciaFechamento(turma.Id, long.Parse(obterComponenteCurricular.Codigo), professorTurma.ProfessorRf, periodoEscolar.Bimestre, obterComponenteCurricular.Descricao, obterComponenteCurricular.LancaNota);
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        private async Task<bool> ExistePendenciaProfessor(long turmaId, long componenteCurricularId, string professorRf)
           => await mediator.Send(new ExistePendenciaProfessorPorTurmaEComponenteQuery(turmaId,
                                                                                       componenteCurricularId,
                                                                                       professorRf,
                                                                                       TipoPendencia.AusenciaFechamento));

        private async Task IncluirPendenciaAusenciaFechamento(long turmaId, long componenteCurricularId, string professorRf, int bimestre, string componenteCurricularNome, bool lancaNota)
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
            

            await mediator.Send(new SalvarPendenciaAusenciaFechamentoCommand(turma.Id, componenteCurricularId, professorRf, titulo, descricao, instrucao));

        }

        private async Task<DisciplinaDto> ObterComponenteCurricular(long componenteCurricularId)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new[] { componenteCurricularId }));
            return componentes.FirstOrDefault();
        }

    }
}
