using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroSistemaUnicoChaveEValorPorTipoQuery : IRequest<KeyValuePair<string, string>?>
    {
        public ObterParametroSistemaUnicoChaveEValorPorTipoQuery(TipoParametroSistema tipo)
        {
            Tipo = tipo;
        }
        public TipoParametroSistema Tipo { get; set; }
    }

    public class ObterParametroSistemaUnicoChaveEValorPorTipoQueryValidator : AbstractValidator<ObterParametroSistemaUnicoChaveEValorPorTipoQuery>
    {
        public ObterParametroSistemaUnicoChaveEValorPorTipoQueryValidator()
        {
            RuleFor(a => a.Tipo)
                .NotEmpty()
                .WithMessage("O tipo do parâmetro precisa ser informado");
        }
    }
}