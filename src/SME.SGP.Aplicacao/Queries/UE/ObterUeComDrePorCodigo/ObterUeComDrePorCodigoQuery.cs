using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUeComDrePorCodigoQuery : IRequest<Ue>
    {
        public ObterUeComDrePorCodigoQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; set; }
    }

    public class ObterUeComDrePorCodigoQueryValidator : AbstractValidator<ObterUeComDrePorCodigoQuery>
    {
        public ObterUeComDrePorCodigoQueryValidator()
        {
            RuleFor(c => c.UeCodigo)
            .NotEmpty()
            .WithMessage("O código da Ue deve ser informado para pesquisa da mesma.");
        }
    }
}
