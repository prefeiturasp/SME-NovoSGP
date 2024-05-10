using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoComAulaETurmaPorCodigoQuery: IRequest<DiarioBordo>
    {
        public ObterDiarioBordoComAulaETurmaPorCodigoQuery(long diarioBordoId)
        {
            DiarioBordoId = diarioBordoId;
        }

        public long DiarioBordoId { get; set; }
    }

    public class ObterDiarioBordoComAulaETurmaPorCodigoQueryValidator : AbstractValidator<ObterDiarioBordoComAulaETurmaPorCodigoQuery>
    {
        public ObterDiarioBordoComAulaETurmaPorCodigoQueryValidator()
        {
            RuleFor(c => c.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O id do diário de bordo deve ser informado.");           
        }
    }
}
