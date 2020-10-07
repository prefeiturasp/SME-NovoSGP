using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroSistemaPorTipoQuery : IRequest<string>
    {
        public ObterParametroSistemaPorTipoQuery(TipoParametroSistema tipo)
        {
            Tipo = tipo;
        }
        public TipoParametroSistema Tipo { get; set; }
    }

    public class ObterParametroSistemaPorTipoQueryValidator : AbstractValidator<ObterParametroSistemaPorTipoQuery>
    {
        public ObterParametroSistemaPorTipoQueryValidator()
        {
            RuleFor(a => a.Tipo)
                .NotEmpty()
                .WithMessage("O tipo do parâmetro precisa ser informado");
        }
    }
}
