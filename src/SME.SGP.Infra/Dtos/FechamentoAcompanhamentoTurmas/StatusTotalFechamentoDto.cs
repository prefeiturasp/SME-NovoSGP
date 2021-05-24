using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class StatusTotalFechamentoDto
    {
        public StatusFechamento Status { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
    }
}
