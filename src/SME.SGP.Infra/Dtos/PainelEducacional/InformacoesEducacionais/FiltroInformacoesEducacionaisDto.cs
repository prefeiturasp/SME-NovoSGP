namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroInformacoesEducacionais : FiltroPaginacaoDto
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; } = null;
    }
}
