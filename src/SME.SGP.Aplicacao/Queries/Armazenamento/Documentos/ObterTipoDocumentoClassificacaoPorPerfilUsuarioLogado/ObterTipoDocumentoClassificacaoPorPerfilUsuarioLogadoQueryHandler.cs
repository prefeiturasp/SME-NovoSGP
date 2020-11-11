using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQueryHandler : IRequestHandler<ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery, IEnumerable<TipoDocumentoDto>>
    {
        private readonly IRepositorioTipoDocumento repositorioTipoDocumento;

        public ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQueryHandler(IRepositorioTipoDocumento repositorioTipoDocumento)
        {
            this.repositorioTipoDocumento = repositorioTipoDocumento ?? throw new ArgumentNullException(nameof(repositorioTipoDocumento));
        }

        public async Task<IEnumerable<TipoDocumentoDto>> Handle(ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery request, CancellationToken cancellationToken)
            => await repositorioTipoDocumento.ListarDocumentosPorPerfil(request.Perfil);
    }
}
