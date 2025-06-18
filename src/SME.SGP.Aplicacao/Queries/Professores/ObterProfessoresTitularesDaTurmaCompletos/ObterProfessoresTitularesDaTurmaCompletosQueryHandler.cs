

using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDaTurmaQueryCompletosHandler : IRequestHandler<ObterProfessoresTitularesDaTurmaCompletosQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private readonly IMediator mediator;

        public ObterProfessoresTitularesDaTurmaQueryCompletosHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDaTurmaCompletosQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<ProfessorTitularDisciplinaEol>();
            var professores = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(request.CodigoTurma, realizaAgrupamento: request.RealizarAgrupamento));
            if (!professores?.Any() ?? true) return retorno;

            if (professores != null)
            {
                foreach (var professor in professores)
                {
                    if (!professor.ProfessorRf.Contains(","))
                    {
                        retorno.Add(professor);
                        continue;
                    }

                    AdicionarProfessoresConcatenados(professor, retorno);
                }
            }

            return retorno;
        }

        private void AdicionarProfessoresConcatenados(ProfessorTitularDisciplinaEol professor, List<ProfessorTitularDisciplinaEol> retorno)
        {
            var professorRfs = professor.ProfessorRf.Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var professorNome = professor.ProfessorNome.Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (var index = 0; index < professorRfs.Count(); index++)
            {
                retorno.Add(new ProfessorTitularDisciplinaEol
                {
                    CodigosDisciplinas = professor.CodigosDisciplinas,
                    DisciplinaNome = professor.DisciplinaNome,
                    ProfessorNome = professorNome[index].Trim(),
                    ProfessorRf = professorRfs[index].Trim(),
                    TurmaId = professor.TurmaId
                });
            }
        }
    }
}