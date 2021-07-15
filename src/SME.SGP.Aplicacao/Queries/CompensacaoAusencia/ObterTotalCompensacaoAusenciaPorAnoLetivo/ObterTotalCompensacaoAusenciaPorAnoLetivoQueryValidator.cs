using FluentValidation;
using SME.SGP.Dominio;
using System.Linq;

namespace SME.SGP.Aplicacao.Queries.EscolaAqui.ObterComunicadosParaFiltro
{
    public class ObterTotalCompensacaoAusenciaPorAnoLetivoQueryValidator : AbstractValidator<ObterTotalCompensacaoAusenciaPorAnoLetivoQuery>
    {
        public ObterTotalCompensacaoAusenciaPorAnoLetivoQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo é obrigatório.")
                .Must(x => x.ToString().Length == 4)
                .WithMessage("O ano letivo informado é inválido.")  
        }
    }
}