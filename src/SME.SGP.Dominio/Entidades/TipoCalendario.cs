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

        public int QuantidadeDeBimestres(Aplicacao? aplicacao = null)
        {
            if (Modalidade.EhEjaOuCelp() && AnoLetivo > 2021)
                return 2;

            if (aplicacao == Aplicacao.SondagemAplicacao || aplicacao == Aplicacao.SondagemDigitacao)
                return 5;

            return 4;
        }
    }
}