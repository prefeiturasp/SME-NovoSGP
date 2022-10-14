using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComMatriculasValidasQueryValidator : AbstractValidator<ObterTurmasComMatriculasValidasQuery>
    {
        public ObterTurmasComMatriculasValidasQueryValidator()
        {
            RuleFor(c => c.AlunoCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código do aluno deve ser informado para obter as turmas com matrículas válidas.");

            RuleFor(c => c.TurmasCodigos)
                .NotNull()
                .WithMessage("Os códigos das turmas devem ser informados para obter as turmas com matrículas válidas.");

            RuleFor(c => c.PeriodoInicio)
                .NotNull()
                .WithMessage("O período de ínicio deve ser informado para obter as turmas com matrículas válidas.");

            RuleFor(c => c.PeriodoFim)
                .NotNull()
                .WithMessage("O período de fim deve ser informado para obter as turmas com matrículas válidas.");
        }
    }
}