using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQuery : IRequest<IEnumerable<CompensacaoAusenciaAlunoEDataDto>>
    {
        public long AulaId { get; set; }

        public ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }        
    }

    public class ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQueryValidator : AbstractValidator<ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQuery>
    {
        public ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQueryValidator()
        {
            RuleFor(x => x.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informada para a obter a compensação de ausência alunos e data.");
        }
    }
}