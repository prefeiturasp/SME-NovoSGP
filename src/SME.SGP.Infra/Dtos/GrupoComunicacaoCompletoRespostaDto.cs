namespace SME.SGP.Infra
{
    public class GrupoComunicacaoCompletoRespostaDto : GrupoComunicacaoCompletoDto
    {
        public string CicloEnsino { get; set; }
        public int? CodCicloEnsino { get; set; }
        public int? CodTipoEscola { get; set; }
        public int? IdCicloEnsino { get; set; }
        public int? IdTipoEscola { get; set; }
        public string TipoEscola { get; set; }
    }
}