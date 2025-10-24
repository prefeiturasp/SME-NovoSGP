using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PaginacaoNotaResultadoDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PaginaAtual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
    }
}