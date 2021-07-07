using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresEventoLiberacaoBoletimQuery : IRequest<int[]>
    {
        public ObterBimestresEventoLiberacaoBoletimQuery(long tipoCalendarioId, DateTime dataRefencia)
        {
            TipoCalendarioId = tipoCalendarioId;
            DataRefencia = dataRefencia;
        }

        public long TipoCalendarioId { get; set; }

        public DateTime DataRefencia { get; set; }
    }
}
