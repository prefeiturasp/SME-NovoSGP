using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentosPorDreUeETipoEClassificacaoQuery : IRequest<PaginacaoResultadoDto<DocumentoResumidoDto>>
    {
        public ObterDocumentosPorDreUeETipoEClassificacaoQuery(long? dreId, long? ueId, long tipoDocumentoId, long classificacaoId, int? anoLetivo)
        {
            DreId = dreId;
            UeId = ueId;
            TipoDocumentoId = tipoDocumentoId;
            ClassificacaoId = classificacaoId;
            AnoLetivo = anoLetivo;
        }

        public long? UeId { get; }
        public long? DreId { get; }
        public long TipoDocumentoId { get; }
        public long ClassificacaoId { get; }
        public int? AnoLetivo { get; }
    }
}
