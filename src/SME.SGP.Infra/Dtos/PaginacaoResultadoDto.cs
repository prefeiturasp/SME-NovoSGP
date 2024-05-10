using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PaginacaoResultadoDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}