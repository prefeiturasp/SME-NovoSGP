using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaEAlunoEAulaCommand : IRequest<bool>
    {
        public ExcluirCompensacaoAusenciaEAlunoEAulaCommand(IEnumerable<CompensacaoAusenciaAlunoAula> compensacaoAusenciaAlunoAulas)
        {
            CompensacaoAusenciaAlunoAulas = compensacaoAusenciaAlunoAulas;
        }

        public IEnumerable<CompensacaoAusenciaAlunoAula> CompensacaoAusenciaAlunoAulas { get; }
    }

    public class ExcluirCompensacaoAusenciaEAlunoEAulaCommandValidator : AbstractValidator<ExcluirCompensacaoAusenciaEAlunoEAulaCommand>
    {
        public ExcluirCompensacaoAusenciaEAlunoEAulaCommandValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaAlunoAulas)
                .NotEmpty()
                .WithMessage("A lista de compensação de ausência aluno aula deve ser preenchido para excluir as compensações.");
        }
    }
}
