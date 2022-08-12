using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class
        ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQueryValidator : AbstractValidator<
            ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQuery>
    {
        public ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQueryValidator()
        {
            RuleFor(c => c.TurmasCodigos)
                .NotNull()
                .WithMessage(
                    "Os códigos das turmas precisam ser informados para consultar as notas do fechamento do bimestre.");

            RuleFor(c => c.Bimestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage(
                    "O bimestre deve ser informado com um valor maior ou igual a 0(zero) para consultar as notas do fechamento do bimestre.");
        }
    }
}