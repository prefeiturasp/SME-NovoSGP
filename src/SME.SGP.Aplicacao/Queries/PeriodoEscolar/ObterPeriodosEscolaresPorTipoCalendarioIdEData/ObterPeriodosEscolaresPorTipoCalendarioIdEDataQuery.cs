using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery : IRequest<PeriodoEscolar>
    {
        public ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(long tipoCalendarioId, DateTime dataAula)
        {
            TipoCalendarioId = tipoCalendarioId;
            DataAula = dataAula;
        }

        public long TipoCalendarioId { get; set; }
        public DateTime DataAula { get; set; }
    }

    public class ObterPeriodosEscolaresPorTipoCalendarioIdEDataQueryValidator : AbstractValidator<ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery>
    {
        public ObterPeriodosEscolaresPorTipoCalendarioIdEDataQueryValidator()
        {
            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado.");
            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("Data da Aula deve ser informada");
        }
    }
}
