using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaAlunoCommand : IRequest<long>
    {
        public SalvarCompensacaoAusenciaAlunoCommand(CompensacaoAusenciaAluno compensacaoAusenciaAluno)
        {
            CompensacaoAusenciaAluno = compensacaoAusenciaAluno;
        }

        public CompensacaoAusenciaAluno CompensacaoAusenciaAluno { get; }
    }

    public class SalvarCompensacaoAusenciaAlunoCommandValidator : AbstractValidator<SalvarCompensacaoAusenciaAlunoCommand>
    {
        public SalvarCompensacaoAusenciaAlunoCommandValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaAluno)
                .NotNull()
                .WithMessage("A compensação ausência aluno deve ser preenchido para salvar");

            RuleFor(x => x.CompensacaoAusenciaAluno.CompensacaoAusenciaId)
                .GreaterThan(0)
                .WithMessage("A compensação ausência id deve ser preenchido para salvar a compensação ausência aluno");

            RuleFor(x => x.CompensacaoAusenciaAluno.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do estudante deve ser preenchido para salvar a compensação ausência aluno");

            RuleFor(x => x.CompensacaoAusenciaAluno.QuantidadeFaltasCompensadas)
                .GreaterThan(0)
                .WithMessage("A quantidade de faltas compensadas deve ser preenchido para salvar a compensação ausência aluno");
        }
    }
}
