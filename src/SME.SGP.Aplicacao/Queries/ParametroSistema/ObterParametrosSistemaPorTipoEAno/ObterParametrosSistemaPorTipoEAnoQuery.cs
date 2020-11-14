using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterParametrosSistemaPorTipoEAnoQuery : IRequest<IEnumerable<ParametrosSistema>>
    {
        public ObterParametrosSistemaPorTipoEAnoQuery(TipoParametroSistema tipoParametroSistema, int ano)
        {
            TipoParametroSistema = tipoParametroSistema;
            Ano = ano;
        }

        public TipoParametroSistema TipoParametroSistema { get; set; }
        public int Ano { get; set; }
    }
    public class ObterParametrosSistemaPorTipoEAnoQueryValidator : AbstractValidator<ObterParametrosSistemaPorTipoEAnoQuery>
    {
        public ObterParametrosSistemaPorTipoEAnoQueryValidator()
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
