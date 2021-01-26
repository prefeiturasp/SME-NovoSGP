using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterChaveEValorPorTipoEAnoQuery : IRequest<IEnumerable<KeyValuePair<string, string>>>
    {
        public ObterChaveEValorPorTipoEAnoQuery(TipoParametroSistema tipo, int ano)
        {
            Tipo = tipo;
            Ano = ano;
        }

        public TipoParametroSistema Tipo { get; set; }
        public int Ano { get; set; }
    }

    public class ObterChaveEValorPorTipoEAnoQueryValidator : AbstractValidator<ObterChaveEValorPorTipoEAnoQuery>
    {
        public ObterChaveEValorPorTipoEAnoQueryValidator()
        {
            RuleFor(a => a.Tipo)
                .NotEmpty()
                .WithMessage("O tipo do parâmetro precisa ser informado");
            RuleFor(a => a.Tipo)
                .NotEmpty()
                .WithMessage("O ano do parâmetro precisa ser informado");
        }
    }
}
