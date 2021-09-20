using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoMediaRegistroIndividualCommand : IRequest<bool>
    {
        public LimparConsolidacaoMediaRegistroIndividualCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; }
    }

    public class LimparConsolidacaoMediaRegistroIndividualCommandValidator : AbstractValidator<LimparConsolidacaoMediaRegistroIndividualCommand>
    {
        public LimparConsolidacaoMediaRegistroIndividualCommandValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para limpar a consolidação de Registro Individual das turmas");
        }
    }
}
