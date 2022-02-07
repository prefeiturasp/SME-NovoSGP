using FluentValidation;

namespace SME.SGP.Infra
{
    public class InserirDiarioBordoDto
    {
        public long AulaId { get; set; }
        public string Planejamento { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class InserirDiarioBordoDtoValidator: AbstractValidator<InserirDiarioBordoDto>
    {
        public InserirDiarioBordoDtoValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                   .NotEmpty()
                   .WithMessage("O Id do Componente Curricular deve ser informado!");
            RuleFor(a => a.AulaId)
                   .NotEmpty()
                   .WithMessage("A aula deve ser informada!");

            RuleFor(a => a.Planejamento)
                   .NotEmpty().WithMessage("O planejamento é obrigatório para o registro do diário de bordo!")
                   .MinimumLength(200).WithMessage("O planejamento deve conter no mínimo 200 caracteres!");
        }
    }
}
