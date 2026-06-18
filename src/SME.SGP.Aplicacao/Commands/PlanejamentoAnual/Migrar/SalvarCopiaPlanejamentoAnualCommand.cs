using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarCopiaPlanejamentoAnualCommand : IRequest<bool>
    {
        public SalvarCopiaPlanejamentoAnualCommand()
        {

        }
        public SalvarCopiaPlanejamentoAnualCommand(PlanejamentoAnual planejamentoAnual)
        {
            PlanejamentoAnual = planejamentoAnual;
        }
        public PlanejamentoAnual PlanejamentoAnual { get; set; }
    }


    public class SalvarCopiaPlanejamentoAnualCommandValidator : AbstractValidator<SalvarCopiaPlanejamentoAnualCommand>
    {
        public SalvarCopiaPlanejamentoAnualCommandValidator()
        {

            RuleFor(c => c.PlanejamentoAnual)
                .NotEmpty()
                .WithMessage("Planejamento anual não informado");
        }
    }
}
