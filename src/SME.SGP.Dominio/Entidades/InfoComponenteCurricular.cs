namespace SME.SGP.Dominio
{
    public class InfoComponenteCurricular
    {
        public long Codigo { get; set; }
        public long? CodigoComponenteCurricularPai { get; set; }
        public bool EhCompartilhada { get; set; }
        public bool EhBaseNacional { get; set; }
        public string Nome { get; set; }
        public string NomeComponenteInfantil { get; set; }
        public bool EhRegencia { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool EhTerritorioSaber { get; set; }
        public bool LancaNota { get; set; }
        public long GrupoMatrizId { get; set; }
        public string GrupoMatrizNome { get; set; }
        public long AreaConhecimentoId { get; set; }
        public string AreaConhecimentoNome { get; set; }
    }
}
