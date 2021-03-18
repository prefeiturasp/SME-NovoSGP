using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoPorAlunoTurmaESemestreQueryValidator : AbstractValidator<ObterAcompanhamentoPorAlunoTurmaESemestreQuery>
    {
        public ObterAcompanhamentoPorAlunoTurmaESemestreQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do Aluno deve ser informado para consulta.");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O Código da Turma deve ser informado para consulta.");

            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O semestre deve ser informado para consulta.");
        }
    }
}
