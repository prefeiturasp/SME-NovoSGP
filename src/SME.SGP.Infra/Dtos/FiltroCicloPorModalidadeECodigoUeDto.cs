namespace SME.SGP.Infra
{
    public class FiltroCicloPorModalidadeECodigoUeDto
    {
        public int Modalidade { get; set; }

        public string CodigoUe { get; set; }

        public bool ConsideraAbrangencia { get; set; }

        public FiltroCicloPorModalidadeECodigoUeDto(int modalidade, string codigoUe, bool consideraAbrangencia)
        {
            Modalidade = modalidade;
            CodigoUe = codigoUe;
            ConsideraAbrangencia = consideraAbrangencia;
        }


    }
}