using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosIdPorCodigosRfQuery : IRequest<IEnumerable<long>>
    {
        public ObterUsuariosIdPorCodigosRfQuery(IList<string> codigosRf)
        {
            CodigosRf = codigosRf;
        }

        public IList<string> CodigosRf { get; set; }
    }

    public class ObterUsuariosIdPorCodigosRfQueryValidator : AbstractValidator<ObterUsuariosIdPorCodigosRfQuery>
    {
        public ObterUsuariosIdPorCodigosRfQueryValidator()
        {
            RuleFor(c => c.CodigosRf)
            .NotEmpty()
            .WithMessage("O CodigoRF deve ser informado para consulta.");
        }
    }
}
