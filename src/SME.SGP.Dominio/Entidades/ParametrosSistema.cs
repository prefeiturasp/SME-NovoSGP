namespace SME.SGP.Dominio
{
    public class ParametrosSistema : EntidadeBase
    {
        public int? Ano { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public TipoParametroSistema Tipo { get; set; }
        public string Valor { get; set; }
    }
}