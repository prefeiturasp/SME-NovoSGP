namespace SME.SGP.Dominio
{
    public class ConfiguracaoEmail : EntidadeBase
    {
        public string EmailRemetente { get; set; }
        public string NomeRemetente { get; set; }
        public int Porta { get; set; }
        public string Senha { get; set; }
        public string ServidorSmtp { get; set; }
        public bool UsarTls { get; set; }
        public string Usuario { get; set; }
    }
}