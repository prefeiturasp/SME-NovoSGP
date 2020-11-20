using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class MigrarPlanejamentoAnualCommand : IRequest<bool>
    {
        public MigrarPlanejamentoAnualCommand(MigrarPlanejamentoAnualDto planejamento)
        {
            Planejamento = planejamento;
        }
        public MigrarPlanejamentoAnualDto Planejamento { get; set; }
    }

    public class MigrarPlanejamentoAnualCommandValidator : AbstractValidator<MigrarPlanejamentoAnualCommand>
    {
        public MigrarPlanejamentoAnualCommandValidator()
        {

            RuleFor(c => c.Planejamento)
                .NotEmpty()
                .WithMessage("O planejamento anual deve ser informado.");
        }
    }
}
