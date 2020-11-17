using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroSistemaPorTipoEAnoQuery : IRequest<ParametrosSistema>
    {
        public ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema tipoParametroSistema, int ano)
        {
            TipoParametroSistema = tipoParametroSistema;
            Ano = ano;
        }

        public TipoParametroSistema TipoParametroSistema { get; set; }
        public int Ano { get; set; }
    }

    public class ObterParametroSistemaPorTipoEAnoQueryValidator : AbstractValidator<ObterParametroSistemaPorTipoEAnoQuery>
    {
        public ObterParametroSistemaPorTipoEAnoQueryValidator()
        {
            RuleFor(c => c.TipoParametroSistema)
            .NotEmpty()
            .WithMessage("O tipo de parâmetro deve ser informado para consulta dos parâmetros do sistema.");

            RuleFor(c => c.Ano)
            .NotEmpty()
            .WithMessage("O ano deve ser informado para consulta dos parâmetros do sistema.");
        }
    }
}
