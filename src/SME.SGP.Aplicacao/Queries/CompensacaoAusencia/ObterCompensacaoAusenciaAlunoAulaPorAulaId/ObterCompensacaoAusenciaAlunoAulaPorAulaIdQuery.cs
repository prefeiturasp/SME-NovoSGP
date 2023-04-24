using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoAulaPorAulaIdQuery : IRequest<IEnumerable<CompensacaoAusenciaAlunoAula>>
    {
        public ObterCompensacaoAusenciaAlunoAulaPorAulaIdQuery(long aulaId, long? numeroAula = null)
        {
            AulaId = aulaId;
            NumeroAula = numeroAula;
        }

        public long AulaId { get; set; }
        public long? NumeroAula { get; set; }
    }

    public class ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQueryValidator : AbstractValidator<ObterCompensacaoAusenciaAlunoAulaPorAulaIdQuery>
    {
        public ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQueryValidator()
        {
            RuleFor(x => x.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informada para a obter a compensação de ausência alunos e data.");
        }
    }
}