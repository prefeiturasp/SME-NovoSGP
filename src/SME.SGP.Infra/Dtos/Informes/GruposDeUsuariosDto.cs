using System;

namespace SME.SGP.Infra.Dtos
{
    public class GruposDeUsuariosDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public Guid GuidPerfil { get; set; }
    }
}
