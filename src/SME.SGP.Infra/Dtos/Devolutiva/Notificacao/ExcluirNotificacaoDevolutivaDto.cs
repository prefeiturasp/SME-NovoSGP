using FluentValidation;

namespace SME.SGP.Infra.Dtos
{
    public class ExcluirNotificacaoDevolutivaDto
    {
        public ExcluirNotificacaoDevolutivaDto(long devolutivaId)
        {
            DevolutivaId = devolutivaId;
        }

        public long DevolutivaId { get; set; }
        
    }

    public class ExcluirNotificacaoDevolutivaDtoValidator : AbstractValidator<ExcluirNotificacaoDevolutivaDto>
    {
        public ExcluirNotificacaoDevolutivaDtoValidator()
        {
            RuleFor(c => c.DevolutivaId)
                .NotEmpty()
                .WithMessage("A devolutiva deve ser informada.");
        }
    }
}
