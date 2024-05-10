using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterReestruturacaoPlanoAEEPorIdQuery : IRequest<PlanoAEEReestruturacao>
    {
        public ObterReestruturacaoPlanoAEEPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ObterReestruturacaoPlanoAEEPorIdQueryValidator : AbstractValidator<ObterReestruturacaoPlanoAEEPorIdQuery>
    {
        public ObterReestruturacaoPlanoAEEPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id da reestruturação do plano deve ser informado para consulta de seus dados.");
        }
    }
}
