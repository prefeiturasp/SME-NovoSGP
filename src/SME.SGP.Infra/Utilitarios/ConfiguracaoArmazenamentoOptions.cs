namespace SME.SGP.Infra.Utilitarios
{
    public class ConfiguracaoArmazenamentoOptions
    {
        public const string Secao = "ConfiguracaoArmazenamento";
        public string BucketTempSGPName { get; set; }
        public string BucketSGP { get; set; }
        public string EndPoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}