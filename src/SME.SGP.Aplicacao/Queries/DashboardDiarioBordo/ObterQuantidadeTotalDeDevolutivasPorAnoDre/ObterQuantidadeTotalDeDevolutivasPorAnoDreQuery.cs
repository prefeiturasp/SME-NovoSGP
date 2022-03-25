using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDevolutivasPorAnoDreQuery : IRequest<IEnumerable<GraficoTotalDevolutivasPorAnoDTO>>
    {
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public long DreId { get; set; }

        public ObterQuantidadeTotalDeDevolutivasPorAnoDreQuery(int anoLetivo, int mes, long dreId)
        {
            AnoLetivo = anoLetivo;
            Mes = mes;
            DreId = dreId;
        }
    }

    public class ObterQuantidadeTotalDeDevolutivasPorAnoDreQueryValidator : AbstractValidator<ObterQuantidadeTotalDeDevolutivasPorAnoDreQuery>
    {
        public ObterQuantidadeTotalDeDevolutivasPorAnoDreQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("É necessário informar o ano letivo para obter o total de devolutivas por ano.");
        }
    }
}
