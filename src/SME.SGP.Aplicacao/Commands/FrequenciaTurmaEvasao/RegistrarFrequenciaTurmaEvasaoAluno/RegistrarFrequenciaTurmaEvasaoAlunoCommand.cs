using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistrarFrequenciaTurmaEvasaoAlunoCommand : IRequest<long>
    {
        public RegistrarFrequenciaTurmaEvasaoAlunoCommand(long frequenciaTurmaEvasaoId, string alunoCodigo, string alunoNome, double percentualFrequencia)
        {
            FrequenciaTurmaEvasaoId = frequenciaTurmaEvasaoId;
            AlunoCodigo = alunoCodigo;
            AlunoNome = alunoNome;
            PercentualFrequencia = percentualFrequencia;
        }

        public long FrequenciaTurmaEvasaoId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public double PercentualFrequencia { get; set; }
    }

    public class RegistrarFrequenciaTurmaEvasaoAlunoCommandValidator : AbstractValidator<RegistrarFrequenciaTurmaEvasaoAlunoCommand>
    {
        public RegistrarFrequenciaTurmaEvasaoAlunoCommandValidator()
        {
            RuleFor(c => c.FrequenciaTurmaEvasaoId)
                .GreaterThan(0)
                .WithMessage("O id da Frequência Turma Evasão deve ser informado.");

            RuleFor(c => c.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");

            RuleFor(c => c.AlunoNome)
                .NotEmpty()
                .WithMessage("O nome do aluno deve ser informado.");
        }
    }
}
