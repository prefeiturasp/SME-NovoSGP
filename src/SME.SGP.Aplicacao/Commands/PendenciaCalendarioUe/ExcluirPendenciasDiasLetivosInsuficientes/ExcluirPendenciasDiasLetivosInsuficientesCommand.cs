using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasDiasLetivosInsuficientesCommand : IRequest<bool>
    {
        public ExcluirPendenciasDiasLetivosInsuficientesCommand(long tipoCalendarioId, string dreCodigo, string ueCodigo)
        {
            TipoCalendarioId = tipoCalendarioId;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
        }

        public long TipoCalendarioId { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
    }
}
