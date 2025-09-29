using FluentValidation;
using SME.SGP.Infra.Consts;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosAtivosPorAnoLetivo
{
    public class ObterAlunosAtivosPorAnoLetivoQueryValidator : AbstractValidator<ObterAlunosAtivosPorAnoLetivoQuery>
    {
        public ObterAlunosAtivosPorAnoLetivoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE)
                .WithMessage($"O ano letivo deve ser maior ou igual a {PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE}.");
        }
    }
}
