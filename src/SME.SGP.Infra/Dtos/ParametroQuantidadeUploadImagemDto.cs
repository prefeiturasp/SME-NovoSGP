namespace SME.SGP.Infra.Dtos
{
    public class ParametroQuantidadeUploadImagemDto
    {
        public int QuantidadeImagemPercursoColetivo { get; set; } = 0;
        public int QuantidadeImagemPercursoIndividual { get; set; } = 0;

        public ParametroQuantidadeUploadImagemDto()
        {
        }

        public void AdicionarValorQuantidadeImagemPercursoColetivo(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
                QuantidadeImagemPercursoColetivo = int.Parse(valor);
        }

        public void AdicionarValorQuantidadeImagemPercursoIndividual(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
                QuantidadeImagemPercursoIndividual = int.Parse(valor);
        }
    }
}