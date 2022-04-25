using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorTurmaEMesQueryValidator : AbstractValidator<ObterFrequenciaAlunosPorTurmaEMesQuery>
    {
        public ObterFrequenciaAlunosPorTurmaEMesQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("A turma precisa ser informada");

            RuleFor(c => c.Mes)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(12)
                .WithMessage("Um mês válido precisa ser informado");
        }
    }
}
