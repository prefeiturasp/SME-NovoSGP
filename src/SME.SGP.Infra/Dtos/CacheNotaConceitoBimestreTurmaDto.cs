using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class CacheNotaConceitoBimestreTurmaDto
    {
        public CacheNotaConceitoBimestreTurmaDto()
        {
            NotasConceitosComponentes = new List<CacheNotaConceitoComponenteDto>();
        }

        public int? Bimestre { get; set; }
        public string TurmaCodigo { get; set; }
        public List<CacheNotaConceitoComponenteDto> NotasConceitosComponentes { get; }
    }
}