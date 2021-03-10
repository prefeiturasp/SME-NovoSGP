using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEPorReestruturacaoIdQuery : IRequest<PlanoAEE>
    {
        public ObterPlanoAEEPorReestruturacaoIdQuery(long reestruturacaoId)
        {
            ReestruturacaoId = reestruturacaoId;
        }

        public long ReestruturacaoId { get; }
    }

    public class ObterPlanoAEEPorReestruturacaoIdQueryValidator : AbstractValidator<ObterPlanoAEEPorReestruturacaoIdQuery>
    {
        public ObterPlanoAEEPorReestruturacaoIdQueryValidator()
        {
            RuleFor(a => a.ReestruturacaoId)
                .NotEmpty()
                .WithMessage("O id da reestruturação deve ser informado para localizar o plano AEE.");
        }
    }
}
