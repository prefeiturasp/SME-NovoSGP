using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFimPeriodoRecorrenciaQuery: IRequest<DateTime>
    {
        public ObterFimPeriodoRecorrenciaQuery(long tipoCalendarioId, DateTime dataInicioRecorrencia, RecorrenciaAula recorrencia)
        {
            TipoCalendarioId = tipoCalendarioId;
            DataInicio = dataInicioRecorrencia;
            Recorrencia = recorrencia;
        }

        public long TipoCalendarioId { get; set; }
        public DateTime DataInicio { get; set; }
        public RecorrenciaAula Recorrencia { get; set; }
    }

    public class ObterFimPeriodoRecorrenciaQueryValidator : AbstractValidator<ObterFimPeriodoRecorrenciaQuery>
    {
        public ObterFimPeriodoRecorrenciaQueryValidator()
        {

            RuleFor(c => c.TipoCalendarioId)
            .NotEmpty()
            .WithMessage("O código do tipo de calendário deve ser informado.");


            RuleFor(c => c.DataInicio)
            .NotEmpty()
            .WithMessage("A data de início da recorrência deve ser informada.");


            RuleFor(c => c.Recorrencia)
            .IsInEnum()
            .WithMessage("O tipo de recorrência deve ser informado.");

        }
    }
}
