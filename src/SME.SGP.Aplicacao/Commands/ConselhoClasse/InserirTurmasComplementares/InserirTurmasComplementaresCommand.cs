using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public  class InserirTurmasComplementaresCommand : IRequest<bool>
    {
        public InserirTurmasComplementaresCommand(long turmaRegularId, long conselhoClasseAlunoId, string alunoCodigo)
        {
            TurmaRegularId = turmaRegularId;
            ConselhoClasseAlunoId = conselhoClasseAlunoId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaRegularId { get; set; }
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

            RuleFor(a => a.TurmaRegularId)
                   .NotEmpty()
                   .WithMessage("O Id da turma regular do aluno deve ser informado!");

        }
    }
}
