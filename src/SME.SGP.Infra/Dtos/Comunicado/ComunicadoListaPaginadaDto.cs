using System;

namespace SME.SGP.Infra
{
    public class ComunicadoListaPaginadaDto
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataEnvio { get; set; }
        public DateTime DataExpiracao { get; set; }
        public int[] ModalidadeCodigo { get; set; }
        public int[] TipoEscolaCodigo { get; set; }
    }
}
