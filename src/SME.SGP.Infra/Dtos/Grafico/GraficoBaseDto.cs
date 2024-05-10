namespace SME.SGP.Infra
{
    public class GraficoBaseDto
    {
        public GraficoBaseDto() { }
        public GraficoBaseDto(string grupo, int quantidade, string descricao)
        {
            Grupo = grupo;
            Quantidade = quantidade;
            Descricao = descricao;
        }

        public string Grupo { get; set; }
        public int Quantidade { get; set; }
        public string Descricao { get; set; }
    }
}
