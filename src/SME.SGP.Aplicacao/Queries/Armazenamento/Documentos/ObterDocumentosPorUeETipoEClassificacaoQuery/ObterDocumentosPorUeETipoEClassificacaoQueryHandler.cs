using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentosPorUeETipoEClassificacaoQueryHandler : ConsultasBase, IRequestHandler<ObterDocumentosPorUeETipoEClassificacaoQuery, PaginacaoResultadoDto<DocumentoResumidoDto>>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public ObterDocumentosPorUeETipoEClassificacaoQueryHandler(IContextoAplicacao contextoAplicacao, 
            IRepositorioDocumento repositorioDocumento) : base(contextoAplicacao)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }

        public async Task<PaginacaoResultadoDto<DocumentoResumidoDto>> Handle(ObterDocumentosPorUeETipoEClassificacaoQuery request, CancellationToken cancellationToken)
        {
            var documentos = await repositorioDocumento.ObterPorUeTipoEClassificacaoPaginada(request.UeId, request.TipoDocumentoId, request.ClassificacaoId, request.AnoLetivo, Paginacao);
            return MapearParaDtoPaginado(documentos);
        }

        private PaginacaoResultadoDto<DocumentoResumidoDto> MapearParaDtoPaginado(PaginacaoResultadoDto<DocumentoResumidoDto> documento)
        {
            var itens = new List<DocumentoResumidoDto>();

            var retornoPaginado = new PaginacaoResultadoDto<DocumentoResumidoDto>
            {
                Items = new List<DocumentoResumidoDto>(),
                TotalPaginas = documento.TotalPaginas,
                TotalRegistros = documento.TotalRegistros
            };

            foreach (var item in documento.Items)
            {
                var comunicadoDto = itens.FirstOrDefault(x => x.DocumentoId == item.DocumentoId);

                if (comunicadoDto.EhNulo())
                    itens.Add(item);
            }

            retornoPaginado.Items = itens;

            return retornoPaginado;
        }
    }
}
