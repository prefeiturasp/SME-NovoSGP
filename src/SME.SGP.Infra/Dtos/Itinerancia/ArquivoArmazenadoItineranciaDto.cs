using System;

namespace SME.SGP.Infra
{
    public class ArquivoArmazenadoItineranciaDto
    {
        public ArquivoArmazenadoItineranciaDto(long id, Guid codigo,string path)
        {
            Id = id;
            Codigo = codigo;
            Path = path;
        }

        public long Id { get; set; }
        public Guid Codigo { get; set; }
        public string Path { get; set; }
    }
}