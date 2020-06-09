using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesDoProfessorCJNaTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesDoProfessorCJNaTurmaQuery, IEnumerable<AtribuicaoCJ>>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ObterComponentesCurricularesDoProfessorCJNaTurmaQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public Task<IEnumerable<AtribuicaoCJ>> Handle(ObterComponentesCurricularesDoProfessorCJNaTurmaQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(request.Login));
        }
    }
}
