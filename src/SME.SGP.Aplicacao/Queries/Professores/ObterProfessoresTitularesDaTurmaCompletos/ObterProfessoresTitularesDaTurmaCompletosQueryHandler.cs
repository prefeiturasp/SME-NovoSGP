

using MediatR;
using SME.SGP.Aplicacao.Integracoes;
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
        private readonly IServicoEol servicoEol;

        public ObterProfessoresTitularesDaTurmaQueryCompletosHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDaTurmaCompletosQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<ProfessorTitularDisciplinaEol>();
            var professores = await servicoEol.ObterProfessoresTitularesDisciplinas(request.CodigoTurma);
            if (!professores?.Any() ?? true) return retorno;

            foreach (var professor in professores)
            {
                if (!professor.ProfessorRf.Contains(","))
                {
                    retorno.Add(professor);
                    continue;
                }

                AdicionarProfessoresConcatenados(professor, retorno);
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
                    DisciplinaId = professor.DisciplinaId,
                    DisciplinaNome = professor.DisciplinaNome,
                    ProfessorNome = professorNome[index].Trim(),
                    ProfessorRf = professorRfs[index].Trim()
                });
            }
        }
    }
}