using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterDataUltimaConsolicacaoDashboardNaapaQuery : IRequest<DateTime?>
    {
        public ObterDataUltimaConsolicacaoDashboardNaapaQuery(TipoParametroSistema tipo)
        {
            Tipo = tipo;
        }

        public TipoParametroSistema Tipo { get; set; }  
    }

    public class ObterDataUltimaConsolicacaoDashboardNaapaQueryValidator : AbstractValidator<ObterDataUltimaConsolicacaoDashboardNaapaQuery>
    {
        public ObterDataUltimaConsolicacaoDashboardNaapaQueryValidator()
        {
            RuleFor(c => c.Tipo)
                .NotNull()
                .WithMessage("O tipo do parametro deve ser informado.");
        }
    }
}
