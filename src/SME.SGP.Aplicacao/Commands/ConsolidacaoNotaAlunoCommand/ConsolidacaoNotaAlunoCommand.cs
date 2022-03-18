using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoNotaAlunoCommand : IRequest<bool>
    {
        public string AlunoCodigo { get; }
        public long TurmaId { get; }
        public int Bimestre { get; }

        public ConsolidacaoNotaAlunoCommand(string alunoCodigo, long turmaId, int bimestre)
        {
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
            Bimestre = bimestre;
        }
    }

    public class PersistirConselhoClasseNotaCommandValidator : AbstractValidator<ConsolidacaoNotaAlunoCommand>
    {
        public PersistirConselhoClasseNotaCommandValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado");

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado");

            RuleFor(a => a.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado");
        }
    }
}
