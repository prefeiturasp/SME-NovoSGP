using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorCodigosUeQuery : IRequest<IEnumerable<Modalidade>>
    {
        public ObterModalidadesPorCodigosUeQuery(string[] codigosUe)
        {
            CodigosUe = codigosUe;
        }
        public string[] CodigosUe { get; set; }

        public class ObterModalidadesPorCodigosUeQueryValidator : AbstractValidator<ObterModalidadesPorCodigosUeQuery>
        {
            public ObterModalidadesPorCodigosUeQueryValidator()
            {
                RuleFor(x => x.CodigosUe).NotNull().WithMessage("CodigosUe deve ser informado");
            }
        }

    }
}
