using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery : IRequest<IEnumerable<FrequenciaAlunoSimplificadoDto>>
    {
        public ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }

    public class ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQueryValidator : AbstractValidator<ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery>
    {
        public ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQueryValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("O id da aula deve ser informado.");
        }
    }

}
