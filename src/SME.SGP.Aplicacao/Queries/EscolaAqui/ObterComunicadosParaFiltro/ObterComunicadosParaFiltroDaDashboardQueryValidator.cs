using FluentValidation;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Queries.EscolaAqui.ObterComunicadosParaFiltro
{
    public class ObterComunicadosParaFiltroDaDashboardQueryValidator : AbstractValidator<ObterComunicadosParaFiltroDaDashboardQuery>
    {
        public ObterComunicadosParaFiltroDaDashboardQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo é obrigatório.")
                .Must(x => x.ToString().Length == 4)
                .WithMessage("O ano letivo informado é inválido.");

            When(x => !string.IsNullOrWhiteSpace(x.CodigoDre) || !string.IsNullOrWhiteSpace(x.CodigoUe), () =>
            {
                RuleFor(x => x.Semestre)
                    .NotEmpty()
                    .When(x => x.Modalidade == Modalidade.EJA)
                    .WithMessage("O semestre é obrigatório.");
            });
        }
    }
}