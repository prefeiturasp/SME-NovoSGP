using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ValidarTipoDocumentoDaClassificacaoQuery : IRequest<bool>
    {
        public ValidarTipoDocumentoDaClassificacaoQuery(long classificacaoDocumentoId, Dominio.Enumerados.TipoDocumento tipoDocumento)
        {
            ClassificacaoDocumentoId = classificacaoDocumentoId;
            TipoDocumento = tipoDocumento;
        }

        public long ClassificacaoDocumentoId { get; set; }
        public Dominio.Enumerados.TipoDocumento TipoDocumento { get; set; }
    }

    public class ValidarTipoDocumentoDaClassificacaoQueryValidator : AbstractValidator<ValidarTipoDocumentoDaClassificacaoQuery>
    {
        public ValidarTipoDocumentoDaClassificacaoQueryValidator()
        {
            RuleFor(c => c.ClassificacaoDocumentoId)
            .NotEmpty()
            .WithMessage("O id da classificação do documento deve ser informado para validação do seu tipo.");

        }
    }
}
