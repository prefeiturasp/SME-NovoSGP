using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaAusenciasPorMotivoQuery : IRequest<IEnumerable<GraficoBaseDto>>
    {
        public ObterDashboardFrequenciaAusenciasPorMotivoQuery(int anoLetivo, long dreId, long ueId, Modalidade? modalidade = null, string ano = "", int semestre = 0)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            Ano = ano;
            Semestre = semestre;
        }

        public int AnoLetivo { get; }
        public long DreId { get; }
        public long UeId { get; }
        public Modalidade? Modalidade { get; }
        public string Ano { get; }
        public int Semestre { get; }
    }

    public class ObterDashboardFrequenciaAusenciasPorMotivoQueryValidator : AbstractValidator<ObterDashboardFrequenciaAusenciasPorMotivoQuery>
    {
        public ObterDashboardFrequenciaAusenciasPorMotivoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta das ausencias com justificativas por motivo");
        }
    }
}
