namespace SME.SGP.Infra
{
    public class DiarioBordoTituloDto
    {
        public DiarioBordoTituloDto(long id, string titulo)
        {
            Id = id;
            Titulo = titulo;
        }

        public long Id { get; set; }
        public string Titulo { get; set; }
      
    }
}
