namespace SME.SGP.Dto
{
    public class AbrangenciaTurmaRetorno
    {
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string Codigo { get; set; }
        public int CodigoModalidade { get; set; }
        public string ModalidadeTurmaNome { get; set; }
        public string Nome { get; set; }
        public int Semestre { get; set; }
        public bool EnsinoEspecial { get; set; }
        public long Id { get; set; }

        public int TipoTurma { get; set; }
    }
}