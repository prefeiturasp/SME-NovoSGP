using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterValorParametroSistemaTipoEAnoQuery : IRequest<string>
    {
        public ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema tipo, int? ano = null)
        {
            Tipo = tipo;
            Ano = ano;
        }

        public TipoParametroSistema Tipo { get; set; }
        public int? Ano { get; set; }
    }

    public class ObterParametroSistemaTipoEAnoQueryValidator: AbstractValidator<ObterValorParametroSistemaTipoEAnoQuery>
    {
        public ObterParametroSistemaTipoEAnoQueryValidator()
        {
            RuleFor(a => a.Tipo)
                .NotEmpty()
                .WithMessage("O tipo do parâmetro precisa ser informado");
        }
    }
}
