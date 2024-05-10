using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPreDefinidasPorTurmaComponenteQuery : IRequest<IEnumerable<FrequenciaPreDefinida>>
    {
        public ObterFrequenciasPreDefinidasPorTurmaComponenteQuery(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class ObterFrequenciasPreDefinidasPorTurmaComponenteQueryValidator : AbstractValidator<ObterFrequenciasPreDefinidasPorTurmaComponenteQuery>
    {
        public ObterFrequenciasPreDefinidasPorTurmaComponenteQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da Turma deve ser informado.");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado.");
        }
    }
}
