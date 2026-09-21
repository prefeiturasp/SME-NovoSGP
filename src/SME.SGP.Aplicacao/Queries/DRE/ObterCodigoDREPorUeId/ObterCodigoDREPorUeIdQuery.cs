using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoDREPorUeIdQuery : IRequest<string>
    {
        public ObterCodigoDREPorUeIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; }
    }

    public class ObterCodigoDREPorUeIdQueryValidator : AbstractValidator<ObterCodigoDREPorUeIdQuery>
    {
        public ObterCodigoDREPorUeIdQueryValidator()
        {
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O id da UE deve ser informado para localizar sua DRE");
        }
    }
}
