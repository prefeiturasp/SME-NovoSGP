using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesDoProfessorNaTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoEol servicoEOL;

        public ObterComponentesCurricularesDoProfessorNaTurmaQueryHandler(IServicoEol servicoEOL, IRepositorioCache repositorioCache)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesDoProfessorNaTurmaQuery request, CancellationToken cancellationToken)
        {
            string nomeChaveCache = $"Componentes-{request.Login}-${request.CodigoTurma}";
            return await repositorioCache.ObterAsync(nomeChaveCache, async () =>
            {
                return await servicoEOL.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(request.CodigoTurma,
                                                                                               request.Login,
                                                                                               request.PerfilUsuario,
                                                                                               request.RealizarAgrupamentoComponente);
            });
        }
    }
}
