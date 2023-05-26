using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class AtendimentosProfissionalEncaminhamentoNAAPAConsolidadoDto
    {
        public AtendimentosProfissionalEncaminhamentoNAAPAConsolidadoDto(long ueId, int anoLetivo, int mes, string profissional, long quantidade)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;  
            Mes = mes;  
            Profissional = profissional;
            Quantidade = quantidade;
        }
        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public string Profissional { get; set; }
        public long Quantidade { get; set; }
    }
}