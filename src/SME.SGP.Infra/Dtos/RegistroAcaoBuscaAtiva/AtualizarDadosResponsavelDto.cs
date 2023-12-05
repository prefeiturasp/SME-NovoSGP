namespace SME.SGP.Infra
{
    public class AtualizarDadosResponsavelDto
    {
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string FoneResidencial { get; set; }
        public string FoneComercial { get; set; }
        public string DDDCelular
        {
            get => string.IsNullOrEmpty(Celular) ? "" : ObterFoneNormalizado(Celular).Substring(0, 2);
        }
        public string NumeroCelular
        {
            get => string.IsNullOrEmpty(Celular) ? "" : ObterFoneNormalizado(Celular)[2..];
        }

        public string DDDResidencial
        {
            get => string.IsNullOrEmpty(FoneResidencial) ? "" : ObterFoneNormalizado(FoneResidencial).Substring(0, 2);
        }
        public string NumeroResidencial
        {
            get => string.IsNullOrEmpty(FoneResidencial) ? "" : ObterFoneNormalizado(FoneResidencial)[2..];
        }

        public string DDDComercial
        {
            get => string.IsNullOrEmpty(FoneComercial) ? "" : ObterFoneNormalizado(FoneComercial).Substring(0, 2);
        }
        public string NumeroComercial
        {
            get => string.IsNullOrEmpty(FoneComercial) ? "" : ObterFoneNormalizado(FoneComercial)[2..];
        }

        private string ObterFoneNormalizado(string fone)
        {
            return string.IsNullOrEmpty(fone) ? "" : fone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
        }
    }
}
