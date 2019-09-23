namespace SME.SGP.Aplicacao.Configuracoes
{
    public class JwtTokenSettings
    {
        public string Audience { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; } = 20;
        public string Issuer { get; set; } = string.Empty;
        public string IssuerSigningKey { get; set; } = string.Empty;
    }
}