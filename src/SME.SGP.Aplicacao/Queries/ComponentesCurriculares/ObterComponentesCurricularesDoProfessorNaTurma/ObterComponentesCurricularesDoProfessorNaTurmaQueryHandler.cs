using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesDoProfessorNaTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IServicoEOL servicoEOL;

        public ObterComponentesCurricularesDoProfessorNaTurmaQueryHandler(IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesDoProfessorNaTurmaQuery request, CancellationToken cancellationToken)
        {
            return await servicoEOL.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(request.CodigoTurma, request.Login, request.PerfilUsuario);
        }
    }
}
