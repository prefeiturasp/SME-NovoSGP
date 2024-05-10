using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPendenciaDiarioBordoPorAulaEComponenteIdQuery : IRequest<long>
    {
        public ObterIdPendenciaDiarioBordoPorAulaEComponenteIdQuery(long aulaId, long componenteCurricularId)
        {
            AulaId = aulaId;
            ComponenteCurricularId = componenteCurricularId;
        }
        public long AulaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
    public class ObterIdPendenciaDiarioBordoQueryValidator : AbstractValidator<ObterIdPendenciaDiarioBordoPorAulaEComponenteIdQuery>
    {
        public ObterIdPendenciaDiarioBordoQueryValidator()
        {
            RuleFor(c => c.AulaId)
               .NotEmpty()
               .WithMessage("O Id da aula ser informado para obter a pendência de diário de bordo.");
            RuleFor(c => c.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O Id do componente curricular deve ser informado para obter a pendência de diário de bordo.");

        }
    }
}

