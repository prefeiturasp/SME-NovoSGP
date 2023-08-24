using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasProgramaAlunoQueryHandler : IRequestHandler<ObterTurmasProgramaAlunoQuery, IEnumerable<AlunoTurmaProgramaDto>>
    {
        private readonly IMediator mediator;

        public ObterTurmasProgramaAlunoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoTurmaProgramaDto>> Handle(ObterTurmasProgramaAlunoQuery request, CancellationToken cancellationToken)
        {
            var turmasProgramaAlunos = new List<AlunoTurmaProgramaDto>();
            Ue ue = null;
            Turma turma = null;
            IEnumerable<DisciplinaResposta> componentesCurriculares = null;

            var estudantes = await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(request.CodigoAluno, request.AnoLetivo, request.FiltrarSituacaoMatricula, false)); ;
            estudantes = estudantes.Where(w => w.CodigoTipoTurma == (int)TipoTurma.Programa);

            foreach (var estudante in estudantes)
            {
                if (turma == null || !estudante.CodigoTurma.ToString().Equals(turma.CodigoTurma))
                {
                    turma = await mediator.Send(new ObterTurmaPorCodigoQuery(estudante.CodigoTurma.ToString()));
                    if (turma == null) continue;

                    componentesCurriculares = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(estudante.CodigoTurma.ToString()));
                }

                if (ue == null || !estudante.CodigoEscola.Equals(ue.CodigoUe))
                    ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(estudante.CodigoEscola));

                var local = ue.TipoEscola != TipoEscola.Nenhum ? $"{ue.TipoEscola.ObterNomeCurto()} {ue.Nome}" : $"{ue.Nome}";

                turmasProgramaAlunos.AddRange(componentesCurriculares.Select(componenteCurricular => new AlunoTurmaProgramaDto()
                {
                    DreUe = $"{ue.Dre.Abreviacao} {local}",
                    Turma = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome} - {((TipoTurnoEOL)turma.TipoTurno).Name()}",
                    ComponenteCurricular = componenteCurricular.Nome
                }));
            }

            return turmasProgramaAlunos.DistinctBy(turmaPrograma => new { turmaPrograma.DreUe, turmaPrograma.Turma, turmaPrograma.ComponenteCurricular });
        }
    }
}