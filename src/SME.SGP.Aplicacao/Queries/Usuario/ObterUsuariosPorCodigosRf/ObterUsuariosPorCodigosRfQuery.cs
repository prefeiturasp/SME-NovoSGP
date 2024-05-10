using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosPorCodigosRfQuery : IRequest<IEnumerable<Usuario>>
    {
        public ObterUsuariosPorCodigosRfQuery(IList<string> codigosRf)
        {
            CodigosRf = codigosRf;
        }

        public IList<string> CodigosRf { get; set; }
    }

    public class ObterUsuariosPorCodigosRfQueryValidator : AbstractValidator<ObterUsuariosPorCodigosRfQuery>
    {
        public ObterUsuariosPorCodigosRfQueryValidator()
        {
            RuleFor(c => c.CodigosRf)
            .NotEmpty()
            .WithMessage("O CodigoRF deve ser informado para consulta.");
        }
    }
}
