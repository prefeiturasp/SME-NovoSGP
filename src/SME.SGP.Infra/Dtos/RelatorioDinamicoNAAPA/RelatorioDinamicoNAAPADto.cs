using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RelatorioDinamicoNAAPADto
    {
        public long TotalRegistro { get; set; }
        public List<TotalRegistroPorModalidadeRelatorioDinamicoNAAPA> TotalRegistroPorModalidades { get; set; }
        public PaginacaoResultadoDto<EncaminhamentoNAAPARelatorioDinamico> EncaminhamentosNAAPAPaginado { get; set; }
    }
}
