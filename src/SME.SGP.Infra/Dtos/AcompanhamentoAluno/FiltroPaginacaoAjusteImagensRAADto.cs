namespace SME.SGP.Infra
{
    public class FiltroPaginacaoAjusteImagensRAADto
    {
        public FiltroPaginacaoAjusteImagensRAADto(int pagina)
        {
            Pagina = pagina;
        }

        public int Pagina { get; set; }
    }
}
