using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaLoginEPerfilQueryHandler : IRequestHandler<ObterComponentesCurricularesPorTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IServicoEol servicoEOL;
        public ObterComponentesCurricularesPorTurmaLoginEPerfilQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorTurmaLoginEPerfilQuery request, CancellationToken cancellationToken)
            => await servicoEOL.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(request.TurmaCodigo, request.UsuarioRf, request.Perfil);
    }
}
