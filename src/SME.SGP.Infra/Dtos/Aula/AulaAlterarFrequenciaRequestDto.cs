using FluentValidation;

namespace SME.SGP.Infra
{
    public class AulaAlterarFrequenciaRequestDto
    {
        public AulaAlterarFrequenciaRequestDto(long aulaId, int quantidadeAnterior)
        {
            AulaId = aulaId;
            QuantidadeAnterior = quantidadeAnterior;
        }

        public long AulaId { get; set; }
        public int QuantidadeAnterior { get; set; }    
    }

    public class AulaAlterarFrequenciaRequestDtoValidator : AbstractValidator<AulaAlterarFrequenciaRequestDto>
    {
        public AulaAlterarFrequenciaRequestDtoValidator()
        {
            RuleFor(c => c.AulaId)
               .NotEmpty()
               .GreaterThan(0)
               .WithMessage("O Id da aula deve ser informado");

            RuleFor(c => c.QuantidadeAnterior)
                .NotEmpty()
                .GreaterThan(1)
                .WithMessage("A quantidade anterior deve ser informada.");

        }
    }

}
