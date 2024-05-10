using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoEscolaPorCodigoUEQuery : IRequest<TipoEscola>
    {
        public ObterTipoEscolaPorCodigoUEQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; }
    }

    public class ObterTipoEscolaPorCodigoUEQueryValidator : AbstractValidator<ObterTipoEscolaPorCodigoUEQuery>
    {
        public ObterTipoEscolaPorCodigoUEQueryValidator()
        {
            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para consulta do Tipo de Escola");
        }
    }
}
