namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroAprovacaoUeDto : FiltroPaginacaoDto
    {
        public int AnoLetivo { get; set; }        
        public string CodigoUe { get; set; } = null;
        public int ModalidadeId { get; set; }
    }
}
