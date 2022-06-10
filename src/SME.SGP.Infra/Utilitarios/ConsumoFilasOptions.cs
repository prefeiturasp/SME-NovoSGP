namespace SME.SGP.Infra.Utilitarios
{
    public class ConsumoFilasOptions
    {
        public const string Secao = "ConsumoFilas";
        public ushort Qos { get; set; }
        public bool Padrao { get; set; }
        public bool Institucional { get; set; }
        public bool Pendencias { get; set; }
        public bool Aula { get; set; }
        public bool Frequencia { get; set; }
        public bool FechamentoConselho { get; set; }
    }
}
