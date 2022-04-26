using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarAnotacaoFechamentoAlunoCommand : IRequest
    {
        public SalvarAnotacaoFechamentoAlunoCommand(AnotacaoFechamentoAluno anotacaoFechamentoAluno)
        {
            AnotacaoFechamentoAluno = anotacaoFechamentoAluno;
        }

        public AnotacaoFechamentoAluno AnotacaoFechamentoAluno { get; }
    }

    public class SalvarAnotacaoFechamentoAlunoCommandValidator : AbstractValidator<SalvarAnotacaoFechamentoAlunoCommand>
    {
        public SalvarAnotacaoFechamentoAlunoCommandValidator()
        {
            RuleFor(a => a.AnotacaoFechamentoAluno)
                .NotEmpty()
                .WithMessage("A entidade de anotação do fechamento do aluno deve ser informada para persistência");

            RuleFor(a => a.AnotacaoFechamentoAluno.FechamentoAlunoId)
                .NotEmpty()
                .WithMessage("O identificador do fechamento do aluno deve ser informada para gerar anotação");

            RuleFor(a => a.AnotacaoFechamentoAluno.Anotacao)
                .NotEmpty()
                .WithMessage("A anotação deve ser informada para vincular ao fechamento do aluno");
        }
    }
}
