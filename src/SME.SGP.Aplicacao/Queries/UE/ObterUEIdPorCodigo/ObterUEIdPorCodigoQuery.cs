using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterUEIdPorCodigoQuery : IRequest<long>
    {
        public ObterUEIdPorCodigoQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; set; }
    }

    public class ObterUEIdPorCodigoQueryValidator : AbstractValidator<ObterUEIdPorCodigoQuery>
    {
        public ObterUEIdPorCodigoQueryValidator()
        {
            RuleFor(c => c.UeCodigo)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado para busca de seu ID.");

        }
    }
}
