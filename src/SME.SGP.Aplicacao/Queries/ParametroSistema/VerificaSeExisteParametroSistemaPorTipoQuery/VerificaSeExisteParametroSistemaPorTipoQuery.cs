using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteParametroSistemaPorTipoQuery :IRequest<bool>
    {
        public VerificaSeExisteParametroSistemaPorTipoQuery(TipoParametroSistema tipo)
        {
            Tipo = tipo;
        }

        public TipoParametroSistema Tipo { get; set; }
    }
    
    public class VerificaSeExisteParametroSistemaPorTipoQueryValidator: AbstractValidator<VerificaSeExisteParametroSistemaPorTipoQuery>
    {
        public VerificaSeExisteParametroSistemaPorTipoQueryValidator()
        {
            RuleFor(x => x.Tipo).NotEmpty().WithMessage("Informe o Tipo do Parametro");
        }
    }
}