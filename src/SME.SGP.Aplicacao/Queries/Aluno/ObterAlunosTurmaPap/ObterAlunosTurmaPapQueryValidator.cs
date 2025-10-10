using FluentValidation;
using SME.SGP.Infra.Consts;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap
{
    public class ObterAlunosTurmaPapQueryValidator : AbstractValidator<ObterAlunosTurmaPapQuery>
    {
        public ObterAlunosTurmaPapQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE)
                .WithMessage($"Ano letivo deve ser maior ou igual a {PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE}.");
        }
    }
}
