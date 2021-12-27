namespace SME.SGP.Infra
{
    public class DiarioBordoTituloDto
    {
        public DiarioBordoTituloDto(long? id, string titulo, bool pendente, long aulaId)
        {
            Id = id;
            Titulo = titulo;
            Pendente = pendente;
            AulaId = aulaId;
        }

        public long? Id { get; set; }
        public string Titulo { get; set; }
        public bool Pendente { get; set; }
        public long AulaId { get; set; }
    }
}
