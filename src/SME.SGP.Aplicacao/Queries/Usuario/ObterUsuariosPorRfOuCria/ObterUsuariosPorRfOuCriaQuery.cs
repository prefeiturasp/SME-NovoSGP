using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public  class ObterUsuariosPorRfOuCriaQuery : IRequest<IEnumerable<Usuario>>
    {
        public ObterUsuariosPorRfOuCriaQuery(IList<string> codigosRf, bool obterPerfis = false)
        {
            CodigosRf = codigosRf;
            ObterPerfis = obterPerfis;
        }

        public IList<string> CodigosRf { get; set; }

        public bool ObterPerfis { get; set; }
    }


    public class ObterUsuariosPorRfOuCriaQueryValidator : AbstractValidator<ObterUsuariosPorRfOuCriaQuery>
    {
        public ObterUsuariosPorRfOuCriaQueryValidator()
        {
            RuleFor(c => c.CodigosRf)
            .NotEmpty()
            .WithMessage("O CodigoRF deve ser informado para consulta.");
        }
    }
}
