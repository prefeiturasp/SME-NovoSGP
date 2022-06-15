using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoAlunoCommand : IRequest
    {
        public SalvarFechamentoAlunoCommand(FechamentoAluno fechamentoAluno)
        {
            FechamentoAluno = fechamentoAluno;
        }

        public FechamentoAluno FechamentoAluno { get; }
    }

    public class SalvarFechamentoAlunoCommandValidator : AbstractValidator<SalvarFechamentoAlunoCommand>
    {
        public SalvarFechamentoAlunoCommandValidator()
        {
            RuleFor(a => a.FechamentoAluno)
                .NotEmpty()
                .WithMessage("A entidade fechamento aluno deve ser informada para persistência");

            RuleFor(a => a.FechamentoAluno.FechamentoTurmaDisciplinaId)
                .NotEmpty()
                .WithMessage("O identificador do fechamento deve ser informado para gerar o fechamento do aluno");

            RuleFor(a => a.FechamentoAluno.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para gerar o fechamento do mesmo");
        }
    }
}
