using FluentValidation;

namespace SME.SGP.Aplicacao
{

    public class SalvarConselhoClasseAlunoNotaCommandValidator : AbstractValidator<SalvarConselhoClasseAlunoNotaCommand>
    {
        public SalvarConselhoClasseAlunoNotaCommandValidator()
        {
            RuleFor(c => c.ConselhoClasseNotaDto)
                .NotNull()
                .WithMessage("As informações da nota/conceito devem ser informados para salvar o conselho de classe.");

            RuleFor(c => c.AlunoCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para salvar o conselho de classe.");

            RuleFor(c => c.CodigoTurma)
                .NotNull()
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para salvar o conselho de classe.");
        }
    }
}