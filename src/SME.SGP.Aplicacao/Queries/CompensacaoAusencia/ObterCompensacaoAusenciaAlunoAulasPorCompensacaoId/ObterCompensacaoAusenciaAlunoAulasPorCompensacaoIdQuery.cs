using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery : IRequest<IEnumerable<Dominio.CompensacaoAusenciaAlunoAula>>
    {
        public ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery(long compensacaoId)
        {
            CompensacaoId = compensacaoId;
        }

        public long CompensacaoId { get; }
    }

    public class ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQueryValidator : AbstractValidator<ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery>
    {
        public ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQueryValidator()
        {
            RuleFor(x => x.CompensacaoId)
                .GreaterThan(0)
                .WithMessage("O código da compensação deve ser preenchido para obter compensação ausência alunos.");
        }
    }
}
