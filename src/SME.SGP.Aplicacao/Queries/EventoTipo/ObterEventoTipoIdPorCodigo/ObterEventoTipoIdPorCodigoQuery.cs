using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterEventoTipoIdPorCodigoQuery : IRequest<long>
    {
        public ObterEventoTipoIdPorCodigoQuery(TipoEvento tipoEvento)
        {
            TipoEvento = tipoEvento;
        }

        public TipoEvento TipoEvento { get; }
    }

    public class ObterEventoTipoIdPorCodigoQueryValidator : AbstractValidator<ObterEventoTipoIdPorCodigoQuery>
    {
        public ObterEventoTipoIdPorCodigoQueryValidator()
        {
            RuleFor(a => a.TipoEvento)
                .NotEmpty()
                .WithMessage("O código do tipo de evento deve ser informado para pesquisa");
        }
    }
}
