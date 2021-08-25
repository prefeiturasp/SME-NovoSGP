using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExisteEventoNaDataPorTipoDreUEQuery : IRequest<bool>
    {
        public ExisteEventoNaDataPorTipoDreUEQuery(DateTime dataReferencia, long tipoCalendarioId, TipoEvento tipoEvento, string dreCodigo, string ueCodigo)
        {
            DataReferencia = dataReferencia;
            TipoCalendarioId = tipoCalendarioId;
            TipoEvento = tipoEvento;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
        }

        public DateTime DataReferencia { get; }
        public long TipoCalendarioId { get; }
        public TipoEvento TipoEvento { get; }
        public string DreCodigo { get; }
        public string UeCodigo { get; }
    }

    public class ExisteEventoNaDataPorTipoDreUEQueryValidator : AbstractValidator<ExisteEventoNaDataPorTipoDreUEQuery>
    {
        public ExisteEventoNaDataPorTipoDreUEQueryValidator()
        {
            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referência deve ser informada para consulta de Evento na data");

            RuleFor(a => a.TipoCalendarioId)
                .NotEmpty()
                .WithMessage("O tipo de calendário deve ser informado para consulta de Evento na data");
        }
    }
}
