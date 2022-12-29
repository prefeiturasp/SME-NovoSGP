using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentosPorUeETipoEClassificacaoQuery : IRequest<PaginacaoResultadoDto<DocumentoResumidoDto>>
    {
        public ObterDocumentosPorUeETipoEClassificacaoQuery(long ueId, long tipoDocumentoId, long classificacaoId, int? anoLetivo)
        {
            UeId = ueId;
            TipoDocumentoId = tipoDocumentoId;
            ClassificacaoId = classificacaoId;
            AnoLetivo = anoLetivo;
        }

        public long UeId { get; }
        public long TipoDocumentoId { get; }
        public long ClassificacaoId { get; }
        public int? AnoLetivo { get; }
    }

    public class ObterDocumentosPorUeETipoEClassificacaoQueryValidator : AbstractValidator<ObterDocumentosPorUeETipoEClassificacaoQuery>
    {
        public ObterDocumentosPorUeETipoEClassificacaoQueryValidator()
        {
            RuleFor(c => c.UeId)
                .NotEmpty()
                .WithMessage("O id da ue deve ser informado.");
        }
    }
}
