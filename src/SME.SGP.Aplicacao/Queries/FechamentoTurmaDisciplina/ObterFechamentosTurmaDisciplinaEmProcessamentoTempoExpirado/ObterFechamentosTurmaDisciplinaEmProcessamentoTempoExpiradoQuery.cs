using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentosTurmaDisciplinaEmProcessamentoTempoExpiradoQuery : IRequest<IEnumerable<(long fechamentoTurmaDisciplinaId, int bimestre, string codigoRf)>>
    {
        public ObterFechamentosTurmaDisciplinaEmProcessamentoTempoExpiradoQuery(DateTime? dataInicio, int? tempoConsideradoExpiradoEmMinutos)
        {
            DataInicio = !dataInicio.HasValue || dataInicio.Value == DateTime.MinValue ? new DateTime(DateTime.Today.Year, 1, 1) : dataInicio.Value.Date;
            TempoConsideradoExpiradoEmMinutos = !tempoConsideradoExpiradoEmMinutos.HasValue ? 60 : tempoConsideradoExpiradoEmMinutos.Value;
        }

        public DateTime DataInicio { get; private set; }
        public int TempoConsideradoExpiradoEmMinutos { get; private set; }
    }
}
