using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisBimestresAlunoQueryValidator : AbstractValidator<ObterNotasFinaisBimestresAlunoQuery>
    {
        public ObterNotasFinaisBimestresAlunoQueryValidator()
        {
            RuleFor(c => c.TurmasCodigos)
                .NotNull()
                .WithMessage("Os códigos das turmas precisam ser informados para consultar as notas finais.");

            RuleFor(c => c.AlunoCodigo)
                .NotNull()
                .WithMessage("Os código do aluno precisa ser informado para consultar as notas finais.");

            RuleFor(c => c.Bimestre)
                .GreaterThanOrEqualTo(0)
                .WithMessage(
                    "O bimestre deve ser informado com um valor maior ou igual a 0(zero) para consultar as notas finais.");
        }
    }
}