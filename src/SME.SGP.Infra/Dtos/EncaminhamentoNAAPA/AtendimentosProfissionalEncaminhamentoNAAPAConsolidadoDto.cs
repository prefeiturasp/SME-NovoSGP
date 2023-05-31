using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class AtendimentosProfissionalEncaminhamentoNAAPAConsolidadoDto
    {
        public AtendimentosProfissionalEncaminhamentoNAAPAConsolidadoDto(long ueId, int anoLetivo, int mes, string nomeProfissional, string rfProfissional, long quantidade)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;  
            Mes = mes;  
            NomeProfissional = nomeProfissional;
            RfProfissional = rfProfissional;
            Quantidade = quantidade;
        }
        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public string NomeProfissional { get; set; }
        public string RfProfissional { get; set; }
        public long Quantidade { get; set; }
    }
}