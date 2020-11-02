using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReplicarParametrosAnoAnteriorCommand : IRequest<bool>
    {
        public int AnoLetivo { get; set; }
        public ReplicarParametrosAnoAnteriorCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public class ReplicarParametrosAnoAnteriorCommandValidator : AbstractValidator<ReplicarParametrosAnoAnteriorCommand>
        {
            public ReplicarParametrosAnoAnteriorCommandValidator()
            {
                RuleFor(a => a.AnoLetivo)
                       .NotEmpty()
                       .WithMessage("O ano deve ser informado!");
            }
        }
    }
}
