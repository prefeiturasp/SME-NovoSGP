using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAEETurmaAlunoCommand : IRequest<bool>
    {
        public SalvarPlanoAEETurmaAlunoCommand(long planoAEEId, string alunoCodigo)
        {
            PlanoAEEId = planoAEEId;
            AlunoCodigo = alunoCodigo;
        }

        public long PlanoAEEId { get; }
        public string AlunoCodigo { get; }
    }

    public class SalvarPlanoAEETurmaAlunoCommandValidator : AbstractValidator<SalvarPlanoAEETurmaAlunoCommand>
    {
        public SalvarPlanoAEETurmaAlunoCommandValidator()
        {
            RuleFor(t => t.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano aee deve ser informado para salvar as turmas do aluno para o plano aee");

            RuleFor(t => t.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do aluno deve ser informado para salvar as turmas do aluno para o plano aee");
        }
    }
}
