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
    public class ObterTipoDocumentoClassificacaoQueryHandler : IRequestHandler<ObterTipoDocumentoClassificacaoQuery, IEnumerable<TipoDocumentoDto>>
    {
        private readonly IRepositorioTipoDocumento repositorioTipoDocumento;

        public ObterTipoDocumentoClassificacaoQueryHandler(IRepositorioTipoDocumento repositorioTipoDocumento)
        {
            this.repositorioTipoDocumento = repositorioTipoDocumento ?? throw new ArgumentNullException(nameof(repositorioTipoDocumento));
        }

        public async Task<IEnumerable<TipoDocumentoDto>> Handle(ObterTipoDocumentoClassificacaoQuery request, CancellationToken cancellationToken)
            => await repositorioTipoDocumento.ListarTipoDocumentoClassificacao();
    }
}
