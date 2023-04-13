using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaAlunoAulaCommand : IRequest<long>
    {
        public SalvarCompensacaoAusenciaAlunoAulaCommand(CompensacaoAusenciaAlunoAula compensacaoAusenciaAlunoAula)
        {
            CompensacaoAusenciaAlunoAula = compensacaoAusenciaAlunoAula;
        }

        public CompensacaoAusenciaAlunoAula CompensacaoAusenciaAlunoAula { get; }
    }

    public class SalvarCompensacaoAusenciaAlunoAulaCommandValidator : AbstractValidator<SalvarCompensacaoAusenciaAlunoAulaCommand>
    {
        public SalvarCompensacaoAusenciaAlunoAulaCommandValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaAlunoAula)
                .NotNull()
                .WithMessage("A compensação ausência aluno aula deve ser preenchido para salvar");

            RuleFor(x => x.CompensacaoAusenciaAlunoAula.RegistroFrequenciaAlunoId)
                .GreaterThan(0)
                .WithMessage("O registro frequencia aluno id deve ser preenchido para salvar a compensação ausência aluno aula");

            RuleFor(x => x.CompensacaoAusenciaAlunoAula.CompensacaoAusenciaAlunoId)
                .GreaterThan(0)
                .WithMessage("A compensação ausência id deve ser preenchido para salvar a compensação ausência aluno aula");

            RuleFor(x => x.CompensacaoAusenciaAlunoAula.NumeroAula)
                .GreaterThan(0)
                .WithMessage("O numero de aula deve ser preenchido para salvar a compensação ausência aluno aula");

            RuleFor(x => x.CompensacaoAusenciaAlunoAula.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser preenchido para salvar a compensação ausência aluno aula");
        }
    }
}
