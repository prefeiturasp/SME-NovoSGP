using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciasPorAlunoQueryValidator : AbstractValidator<ObterOcorrenciasPorAlunoQuery>
    {
        public ObterOcorrenciasPorAlunoQueryValidator()
        {
            RuleFor(c => c.AlunoCodigo)
            .NotEmpty()
            .WithMessage("O Código do aluno deve ser informado.");

            RuleFor(c => c.PeriodoInicio)
            .NotEmpty()
            .WithMessage("O Periodo Início deve ser informado.");

            RuleFor(c => c.PeriodoInicio)
            .NotEmpty()
            .WithMessage("O Periodo Final deve ser informado.");

            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("A Turma Id deve ser informada.");
        }
    }
}
