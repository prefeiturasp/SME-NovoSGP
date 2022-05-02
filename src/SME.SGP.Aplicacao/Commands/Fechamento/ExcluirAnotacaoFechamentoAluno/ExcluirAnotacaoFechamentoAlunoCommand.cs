using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacaoFechamentoAlunoCommand : IRequest
    {
        public ExcluirAnotacaoFechamentoAlunoCommand(AnotacaoFechamentoAluno anotacaoFechamentoAluno)
        {
            AnotacaoFechamentoAluno = anotacaoFechamentoAluno;
        }

        public AnotacaoFechamentoAluno AnotacaoFechamentoAluno { get; }
    }

    public class ExcluirAnotacaoFechamentoAlunoCommandValidator : AbstractValidator<ExcluirAnotacaoFechamentoAlunoCommand>
    {
        public ExcluirAnotacaoFechamentoAlunoCommandValidator()
        {
            RuleFor(a => a.AnotacaoFechamentoAluno)
                .NotEmpty()
                .WithMessage("A entidade da anotação do fechamento do aluno deve ser informada para exclusão");

            RuleFor(a => a.AnotacaoFechamentoAluno.Id)
                .NotEmpty()
                .WithMessage("O identificador da anotação do fechamento do aluno deve ser informado para exclusão");
        }
    }
}
