namespace SME.SGP.Infra
{
    public class ConsultaDatasAulasDto
    {
        public ConsultaDatasAulasDto(string turmaCodigo, string componenteCurricularCodigo)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
    }
}
