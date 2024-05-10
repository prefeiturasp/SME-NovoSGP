using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQueryValidator : AbstractValidator<ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQuery>
    {
        public ObterAnotacaoFechamentoAlunoPorDisciplinasEAlunosQueryValidator()
        {
            RuleFor(c => c.FechamentosTurmasDisciplinasIds)
                .NotNull()
                .WithMessage("Os ids das disciplinas devem ser informados para a consulta de anotação do fechamento do aluno.");

            RuleFor(c => c.AlunosCodigos)
                .NotNull()
                .WithMessage("Os códigos dos alunos devem ser informados para a consulta de anotação do fechamento do aluno.");
        }
    }
}
