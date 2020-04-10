using System;

namespace SME.SGP.Dto
{
    public class ComunicadoDto
    {
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public int Id { get; set; }
        public string Titulo { get; set; }
    }
}