using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQueryValidator : AbstractValidator<ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery>
    {
        public ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQueryValidator()
        {
            RuleFor(x => x.CodigoDre)
                .NotEmpty()
                .WithMessage("O código da DRE é obrigatório.");

            RuleFor(x => x.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE é obrigatório.");

            RuleFor(x => x.ComunicadoId)
                .NotEmpty()
                .WithMessage("O comunicado é obrigatório.");

            RuleFor(x => x.ModoVisualizacao)
                .NotEmpty()
                .WithMessage("O Modo de Visualização é obrigatório.");
        }
    }
}