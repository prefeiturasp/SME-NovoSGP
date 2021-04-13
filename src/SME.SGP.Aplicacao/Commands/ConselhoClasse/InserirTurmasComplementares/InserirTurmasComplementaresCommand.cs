using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public  class InserirTurmasComplementaresCommand : IRequest<bool>
    {
        public InserirTurmasComplementaresCommand(long turmaId, long conselhoClasseAlunoId, string alunoCodigo)
        {
            TurmaId = turmaId;
            ConselhoClasseAlunoId = conselhoClasseAlunoId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public string AlunoCodigo { get; set; }
    }

    public class InserirTurmasComplementaresCommandValidator : AbstractValidator<InserirTurmasComplementaresCommand>
    {
        public InserirTurmasComplementaresCommandValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                   .NotEmpty()
                   .WithMessage("O código do aluno deve ser informado!");

            RuleFor(a => a.ConselhoClasseAlunoId)
                   .NotEmpty()
                   .WithMessage("O Id do conselho de classe do aluno devem ser informado!");

            RuleFor(a => a.TurmaId)
                   .NotEmpty()
                   .WithMessage("O Id da turma do aluno deve ser informado!");
        }
    }
}
