namespace SME.SGP.Infra
{
    public class OpcaoDropdownDto
    {
        public OpcaoDropdownDto(string valor, string descricao)
        {
            Valor = valor;
            Descricao = descricao;
        }

        public string Valor { get; set; }
        public string Descricao { get; set; }
    }
}
