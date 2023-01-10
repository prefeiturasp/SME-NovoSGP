using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterDocumentoPorIdCompletoQuery : IRequest<ObterDocumentoResumidoDto>
    {
        public ObterDocumentoPorIdCompletoQuery(long documentoId)
        {
            DocumentoId = documentoId;
        }

        public long DocumentoId { get; }
    }

    public class ObterDocumentoPorIdCompletoQueryValidator : AbstractValidator<ObterDocumentoPorIdCompletoQuery>
    {
        public ObterDocumentoPorIdCompletoQueryValidator()
        {
            RuleFor(c => c.DocumentoId)
                .NotEmpty()
                .WithMessage("O id do documento deve ser informado para obter seus dados.");
        }
    }
}
