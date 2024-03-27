using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterDataUltimaConsolicacaoDashboardNaapaQuery : IRequest<DateTime?>
    {
        public ObterDataUltimaConsolicacaoDashboardNaapaQuery(TipoParametroSistema tipo, int anoLetivo)
        {
            Tipo = tipo;
            AnoLetivo = anoLetivo;
        }

        public TipoParametroSistema Tipo { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterDataUltimaConsolicacaoDashboardNaapaQueryValidator : AbstractValidator<ObterDataUltimaConsolicacaoDashboardNaapaQuery>
    {
        public ObterDataUltimaConsolicacaoDashboardNaapaQueryValidator()
        {
            RuleFor(c => c.Tipo)
                .NotNull()
                .WithMessage("O tipo do parametro deve ser informado para obter a data da última consolidação do dash NAAPA.");
            RuleFor(c => c.AnoLetivo)
                .NotNull()
                .WithMessage("O Ano letivo deve ser informado para obter a data da última consolidação do dash NAAPA.");
        }
    } 
}
