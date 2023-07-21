using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos
{
    public class FrequenciaGlobalMensalSemanalDto
    {
        public string NomeTurma { get; set; }
        public int AnoTurma {  get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public string AbreviacaoDre { get; set; }
        public string CodigoDre { get; set; }
        public int QuantidadeAbaixoMinimoFrequencia { get; set; }
        public int QuantidadeAcimaMinimoFrequencia { get; set; }
        public bool VisaoDre { get; set; }
        public long UeId { get; set; }
        public string Descricao { get { return ObterDescricao(); } }

        private string ObterDescricao()
        {
            if (VisaoDre)
                return AbreviacaoDre;
            else if (UeId == -99)
                return $"{ModalidadeTurma.ShortName()}-{AnoTurma}";
            
            return $"{ModalidadeTurma.ShortName()}-{NomeTurma}";
        }
    }
}
