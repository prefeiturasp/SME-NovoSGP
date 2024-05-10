using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class InserirOuAtualizarCacheFechamentoNotaConceitoCommandValidator : AbstractValidator<InserirOuAtualizarCacheFechamentoNotaConceitoCommand>
    {
        public InserirOuAtualizarCacheFechamentoNotaConceitoCommandValidator()
        {
            RuleFor(c => c.ComponenteCurricularId)
                .GreaterThan(0)
                .WithMessage(
                    "O Id do componente curricular deve ser informado para inserir ou atualizar o cache do fechamento.");

            RuleFor(c => c.TurmaCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para inserir ou atualizar o cache do fechamento.");

            RuleFor(c => c.Bimestre)
                .InclusiveBetween(0, 4)
                .When(c => !c.Bimestre.HasValue)
                .WithMessage("Um bimestre válido deve ser informado para inserir ou atualizar o cache do fechamento.");

            RuleFor(c => c.FechamentosNotasConceitos)
                .NotNull()
                .WithMessage(
                    "As notas ou conceitos devem ser informados para inserir ou atualizar o cache do fechamento.")
                .DependentRules(() =>
                {
                    RuleForEach(c => c.FechamentosNotasConceitos)
                        .ChildRules(c =>
                        {
                            c.RuleFor(b => b.CodigoAluno)
                                .NotEmpty()
                                .NotNull()
                                .WithMessage(
                                    "O código do aluno deve ser informado para inserir ou atualizar o cache de fechamento.");
                        });
                });
        }
    }
}