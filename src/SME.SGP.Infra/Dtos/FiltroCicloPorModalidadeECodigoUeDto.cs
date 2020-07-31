namespace SME.SGP.Infra
{
    public class FiltroCicloPorModalidadeECodigoUeDto
    {
        public int Modalidade { get; set; }

        public string CodigoUe { get; set; }

        public FiltroCicloPorModalidadeECodigoUeDto(int modalidade, string codigoUe)
        {
            Modalidade = modalidade;
            CodigoUe = codigoUe;
        }
    }
}