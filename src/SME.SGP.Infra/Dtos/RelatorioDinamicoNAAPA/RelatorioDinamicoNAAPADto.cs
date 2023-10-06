using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RelatorioDinamicoNAAPADto
    {
        public long TotalRegistro { get; set; }
        public IEnumerable<TotalRegistroPorModalidadeRelatorioDinamicoNAAPA> TotalRegistroPorModalidadesAno { get; set; }
        public PaginacaoResultadoDto<EncaminhamentoNAAPARelatorioDinamico> EncaminhamentosNAAPAPaginado { get; set; }
    }
}
