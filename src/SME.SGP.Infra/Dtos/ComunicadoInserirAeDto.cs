using System;

namespace SME.SGP.Dto
{
    public class ComunicadoInserirAeDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Grupo { get; set; }
        public long Id { get; set; }
        public string Mensagem { get; set; }
        public string Titulo { get; set; }        
    }
}