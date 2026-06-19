using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterTamanhoCaracteresJustificativaNotaQuery : IRequest<int>
    {
        public string Justificativa { get; set; }

        public ObterTamanhoCaracteresJustificativaNotaQuery(string justificativa)
        {
            Justificativa = justificativa;
        }
    }

    public class ObterTamanhoCaracteresJustificativaNotaQueryValidator : AbstractValidator<ObterTamanhoCaracteresJustificativaNotaQuery>
    {
        public ObterTamanhoCaracteresJustificativaNotaQueryValidator()
        {
            RuleFor(a => a.Justificativa)
                .NotEmpty()
                .WithMessage("É necessário informar o texto da justificativa para verificar o tamanho se está no limite de caracteres.");
        }
    }


}
