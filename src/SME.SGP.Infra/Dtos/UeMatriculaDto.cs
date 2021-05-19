namespace SME.SGP.Infra
{
    public class UeMatriculaDto
    {
        public UeMatriculaDto(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; set; }
    }
}
