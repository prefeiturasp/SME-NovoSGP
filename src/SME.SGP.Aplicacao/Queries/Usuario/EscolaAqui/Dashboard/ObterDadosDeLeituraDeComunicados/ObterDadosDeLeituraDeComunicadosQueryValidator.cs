using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosQueryValidator : AbstractValidator<ObterDadosDeLeituraDeComunicadosQuery>
    {
        public ObterDadosDeLeituraDeComunicadosQueryValidator()
        {
            RuleFor(x => x.ComunicadoId)
                .NotEmpty()
                .WithMessage("O comunicado é obrigatório.");

            RuleFor(x => x.ModoVisualizacao)
                .NotEmpty()
                .WithMessage("O Modo de Visualização é obrigatório.");
        }
    }
}