using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterClassificacaoDocumentoPorIdQueryValidator : AbstractValidator<ObterClassificacaoDocumentoPorIdQuery>
    {
        public ObterClassificacaoDocumentoPorIdQueryValidator()
        {
            RuleFor(c => c.ClassificacaoId)
                .GreaterThan(0)
                .WithMessage(
                    "O Id da classificação do documento deve ser informado para obter a classificação do documento.");
        }
    }
}