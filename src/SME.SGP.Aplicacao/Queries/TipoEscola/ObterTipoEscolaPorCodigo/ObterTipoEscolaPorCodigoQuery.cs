using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoEscolaPorCodigoQuery : IRequest<TipoEscolaEol>
    {
        public ObterTipoEscolaPorCodigoQuery(long codigo)
        {
            Codigo = codigo;
        }

        public long Codigo { get; set; }
    }
    public class ObterTipoEscolaPorCodigoQueryValidator : AbstractValidator<ObterTipoEscolaPorCodigoQuery>
    {
        public ObterTipoEscolaPorCodigoQueryValidator()
        {
            RuleFor(c => c.Codigo)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("O código deve ser informado.");
           
        }
    }
}
