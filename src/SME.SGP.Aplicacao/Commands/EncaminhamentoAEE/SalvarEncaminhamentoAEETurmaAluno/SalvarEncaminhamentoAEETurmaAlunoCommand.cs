using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarEncaminhamentoAEETurmaAlunoCommand : IRequest<bool>
    {
        public SalvarEncaminhamentoAEETurmaAlunoCommand(long encaminhamentoAEEId, string alunoCodigo)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
            AlunoCodigo = alunoCodigo;
        }

        public long EncaminhamentoAEEId { get; }
        public string AlunoCodigo { get; }
    }

    public class SalvarEncaminhamentoAEETurmaAlunoCommandValidator : AbstractValidator<SalvarEncaminhamentoAEETurmaAlunoCommand>
    {
        public SalvarEncaminhamentoAEETurmaAlunoCommandValidator()
        {
            RuleFor(t => t.EncaminhamentoAEEId)
                .NotEmpty()
                .WithMessage("O id do encaminhamento aee deve ser informado para salvar as turmas do aluno para o encaminhamento aee");

            RuleFor(t => t.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do aluno deve ser informado para salvar as turmas do aluno para o encaminhamento aee");
        }
    }
}
