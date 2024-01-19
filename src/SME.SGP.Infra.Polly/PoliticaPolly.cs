namespace SME.SGP.Infra
{
    public abstract class PoliticaPolly
    {
        protected PoliticaPolly() { }
        public static string SGP => "RetryPolicySGP";
        public static string PublicaFila => "RetryPolicyFilasRabbit";
    }
}
