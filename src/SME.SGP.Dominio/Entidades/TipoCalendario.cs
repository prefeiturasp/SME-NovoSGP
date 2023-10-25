namespace SME.SGP.Dominio
{
    public class TipoCalendario : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public string Nome { get; set; }
        public Periodo Periodo { get; set; }
        public bool Situacao { get; set; }
        public int? Semestre { get; set; }

        public int QuantidadeDeBimestres()
        {
            if (Modalidade.EhEjaOuCelp() && AnoLetivo > 2021)
                return 2;
 
            return 4;
        }
    }
}