using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterUeCodigoPorIdQuery : IRequest<string>
    {
        public ObterUeCodigoPorIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; }
    }

    public class ObterUeCodigoPorIdQueryValidator : AbstractValidator<ObterUeCodigoPorIdQuery>
    {
        public ObterUeCodigoPorIdQueryValidator()
        {
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O identificador da UE é obrigatorio para localizar o código");
        }
    }
}
