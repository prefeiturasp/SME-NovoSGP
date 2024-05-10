namespace SME.SGP.Infra
{
    public class UesOcorreciaDto
    {
        public UesOcorreciaDto(long idUe, string nomeUe)
        {
            IdUe = idUe;
            NomeUe = nomeUe;
        }

        public long IdUe { get; set; }
        public string NomeUe { get; set; }
    }
}