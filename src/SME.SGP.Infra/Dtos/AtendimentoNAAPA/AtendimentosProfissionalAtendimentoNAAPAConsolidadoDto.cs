using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto
    {
        public AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto(long ueId, int anoLetivo, int mes, string nomeProfissional, string rfProfissional, long quantidade, Modalidade modalidade)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;  
            Mes = mes;  
            NomeProfissional = nomeProfissional;
            RfProfissional = rfProfissional;
            Quantidade = quantidade;
            Modalidade = modalidade;
        }
        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public string NomeProfissional { get; set; }
        public string RfProfissional { get; set; }
        public long Quantidade { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}