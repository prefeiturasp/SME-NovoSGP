using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery : IRequest<IEnumerable<long>>
    {
        public ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery(string ueId)
        {
            UeId = ueId;
        }

        public string UeId { get; set; }
    }
    public class ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQueryValidator : AbstractValidator<ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQuery>
    {
        public ObterCodigosTurmasEOLPorUeIdParaSyncEstruturaInstitucionalQueryValidator()
        {
            RuleFor(c => c.UeId)
                .NotEmpty()
                .WithMessage("O id da Ue deve ser informado.");
        }
    }
}
