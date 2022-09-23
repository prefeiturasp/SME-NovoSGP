namespace SME.SGP.Dados.ElasticSearch
{
    public class ElasticOptions
    {
        public string Urls { get; set; }
        public string DefaultIndex { get; set; }
        public string PrefixIndex { get; set; }
        public string CertificateFingerprint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
