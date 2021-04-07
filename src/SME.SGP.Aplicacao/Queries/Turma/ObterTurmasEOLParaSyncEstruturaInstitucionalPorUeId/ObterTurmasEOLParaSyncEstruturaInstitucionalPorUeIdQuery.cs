using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQuery(string ueId)
        {
            UeId = ueId;
        }

        public string UeId { get; set; }
    }
    public class ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQueryValidator : AbstractValidator<ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQuery>
    {
        public ObterTurmasEOLParaSyncEstruturaInstitucionalPorUeIdQueryValidator()
        {

            RuleFor(c => c.UeId)
                .NotEmpty()
                .WithMessage("O id da Ue deve ser informado.");
        }
    }
}
