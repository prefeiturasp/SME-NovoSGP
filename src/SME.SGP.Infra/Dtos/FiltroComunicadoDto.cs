using System;

namespace SME.SGP.Infra
{
    public class FiltroComunicadoDto
    {
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Titulo { get; set; }
    }
}