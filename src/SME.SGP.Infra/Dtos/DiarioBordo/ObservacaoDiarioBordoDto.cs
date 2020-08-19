using FluentValidation;

namespace SME.SGP.Infra
{
    public class ObservacaoDiarioBordoDto
    {
        public long ObservacaoId { get; set; }
        public string Observacao { get; set; }
    }

    public class ObservacaoDiarioBordoDtoValidator : AbstractValidator<ObservacaoDiarioBordoDto>
    {
        public ObservacaoDiarioBordoDtoValidator()
        {
            RuleFor(c => c.Observacao)
                .NotEmpty()
                .WithMessage("A observação deve ser informada.");
        }
    }
}
