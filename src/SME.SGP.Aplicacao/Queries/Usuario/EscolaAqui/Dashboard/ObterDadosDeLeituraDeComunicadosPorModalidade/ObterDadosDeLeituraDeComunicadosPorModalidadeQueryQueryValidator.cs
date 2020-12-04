using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadeQueryValidator : AbstractValidator<ObterDadosDeLeituraDeComunicadosPorModalidadeQuery>
    {
        public ObterDadosDeLeituraDeComunicadosPorModalidadeQueryValidator()
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