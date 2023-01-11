using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class
        ObterDocumentosArquivosPorDocumentoIdQueryValidator : AbstractValidator<ObterDocumentosArquivosPorDocumentoIdQuery>
    {
        public ObterDocumentosArquivosPorDocumentoIdQueryValidator()
        {
            RuleFor(c => c.DocumentoId)
                .GreaterThan(0)
                .WithMessage("O Id do documento deve ser informado para obter os arquivos do documento.");
        }
    }
}