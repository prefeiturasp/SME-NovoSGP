using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoBimestrePorDreUeEDataQuery : IRequest<PeriodoFechamentoBimestre>
    {
        public ObterPeriodoFechamentoBimestrePorDreUeEDataQuery(ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataInicio, int bimestre, long? dreId = null, long? ueId = null)
        {
            DataInicio = dataInicio;
            DreId = dreId;
            UeId = ueId;
            Bimestre = bimestre;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
        }

        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public int Bimestre { get; set; }
        public DateTime DataInicio { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
    }

    public class ObterPeriodoFechamentoBimestrePorDreUeEDataQueryValidator : AbstractValidator<ObterPeriodoFechamentoBimestrePorDreUeEDataQuery>
    {
        public ObterPeriodoFechamentoBimestrePorDreUeEDataQueryValidator()
        {
            RuleFor(c => c.DataInicio)
               .Must(a => a > DateTime.MinValue)
               .WithMessage("A data de inicio de fechamento deve ser informado.");

            RuleFor(c => c.Bimestre)
               .Must(a => a > 0)
               .WithMessage("O bimestre do fechamento deve ser informado.");
        }
    }
}
