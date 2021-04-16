using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCicloPorCodigoQuery : IRequest<CicloEnsino>
    {
        public ObterCicloPorCodigoQuery(long codigoEol)
        {
            CodigoEol = codigoEol;
        }

        public long CodigoEol { get; set; }
    }
    public class ObterCicloPorCodigoQueryValidator : AbstractValidator<ObterCicloPorCodigoQuery>
    {
        public ObterCicloPorCodigoQueryValidator()
        {
            RuleFor(a => a.CodigoEol)
                .NotEmpty()
                .WithMessage("O codígo do ciclo do Eol deve ser informado!");
        }
    }
}
