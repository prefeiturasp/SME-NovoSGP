namespace SME.SGP.Infra
{
    public class OcorrenciaListagemDto
    {
        public long Id { get; set; }
        public string DataOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string AlunoOcorrencia { get; set; }
        public long TurmaId { get; set; }
    }
}