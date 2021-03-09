using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEReestruturacaoPorIdQuery : IRequest<PlanoAEEReestruturacao>
    {
        public ObterPlanoAEEReestruturacaoPorIdQuery(long reestruturacaoId)
        {
            ReestruturacaoId = reestruturacaoId;
        }

        public long ReestruturacaoId { get; }
    }

    public class ObterPlanoAEEReestruturacaoPorIdQueryValidator : AbstractValidator<ObterPlanoAEEReestruturacaoPorIdQuery>
    {
        public ObterPlanoAEEReestruturacaoPorIdQueryValidator()
        {
            RuleFor(a => a.ReestruturacaoId)
                .NotEmpty()
                .WithMessage("O id da reestruturação deve ser informado para consulta.");
        }
    }
}
