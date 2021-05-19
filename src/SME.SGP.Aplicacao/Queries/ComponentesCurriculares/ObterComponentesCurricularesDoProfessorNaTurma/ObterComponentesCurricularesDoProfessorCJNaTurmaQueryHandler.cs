using System.Collections;
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
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ObterComponentesCurricularesDoProfessorCJNaTurmaQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IRepositorioCache repositorioCache)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<AtribuicaoCJ>> Handle(ObterComponentesCurricularesDoProfessorCJNaTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(
                $"AtribuicaoAtiva-{request.Login}", 
                async () => await repositorioAtribuicaoCJ.ObterAtribuicaoAtivaAsync(request.Login));
        }
    }
}
