using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasIdPorUeCodigoEAnoLetivoQuery : IRequest<IEnumerable<long>>
    {
        public ObterTurmasIdPorUeCodigoEAnoLetivoQuery(int anoLetivo, string ueCodigo)
        {
            AnoLetivo = anoLetivo;
            UeCodigo = ueCodigo;
        }

        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
    }
}
