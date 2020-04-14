namespace SME.SGP.Infra
{
    public class CalendarioEventosNoDiaRetornoDto
    {
        public long Id { get; set; }
        public string InicioFimDesc { get; set; }
        public string Nome { get { return $"{_nome} {InicioFimDesc}"; } set { _nome = value; } }
        public string TipoEvento { get; set; }
        private string _nome { get; set; }
    }
}