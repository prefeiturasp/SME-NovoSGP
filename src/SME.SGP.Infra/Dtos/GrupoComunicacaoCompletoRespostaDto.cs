namespace SME.SGP.Infra
{
    public class GrupoComunicacaoCompletoRespostaDto : GrupoComunicacaoCompletoDto
    {
        public string CicloEnsino { get; set; }
        public int CicloEnsinoId { get; set; }
        public string TipoEscola { get; set; }
        public int TipoEscolaId { get; set; }
    }
}