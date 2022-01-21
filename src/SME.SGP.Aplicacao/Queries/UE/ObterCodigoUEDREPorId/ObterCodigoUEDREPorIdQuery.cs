using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoUEDREPorIdQuery : IRequest<DreUeDto>
    {
        public ObterCodigoUEDREPorIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; }
    }

    public class ObterCodigoUEDREPorIdQueryValidator : AbstractValidator<ObterCodigoUEDREPorIdQuery>
    {
        public ObterCodigoUEDREPorIdQueryValidator()
        {
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O id da UE deve ser informado para consulta dos códigos de DRE e UE");
        }
    }
}
