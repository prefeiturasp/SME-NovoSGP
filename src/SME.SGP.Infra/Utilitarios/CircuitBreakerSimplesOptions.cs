namespace SME.SGP.Infra.Utilitarios
{
    public class CircuitBreakerSimplesOptions
    {
        public const string Secao = "CircuitBreakerSimples";

        public int LimiteFalhas { get; set; } = 5;
        public int TempoAberturaSegundos { get; set; } = 30;
    }
}
