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
    public class ObterDocumentosPorUeETipoEClassificacaoQueryHandler : IRequestHandler<ObterDocumentosPorUeETipoEClassificacaoQuery, IEnumerable<DocumentoDto>>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public ObterDocumentosPorUeETipoEClassificacaoQueryHandler(IRepositorioDocumento repositorioDocumento)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }

        public async Task<IEnumerable<DocumentoDto>> Handle(ObterDocumentosPorUeETipoEClassificacaoQuery request, CancellationToken cancellationToken)
            => await repositorioDocumento.ObterPorUeTipoEClassificacao(request.UeId, request.TipoDocumentoId, request.ClassificacaoId);
    }
}
