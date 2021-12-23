namespace SME.SGP.Infra
{
    public class DiarioBordoTituloDto
    {
        public DiarioBordoTituloDto(long? id, string titulo, bool pendente)
        {
            Id = id;
            Titulo = titulo;
            Pendente = pendente;
        }

        public long? Id { get; set; }
        public string Titulo { get; set; }
        public bool Pendente { get; set; }
      
    }
}
