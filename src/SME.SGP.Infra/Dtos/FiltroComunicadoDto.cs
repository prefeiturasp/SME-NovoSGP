using System;

namespace SME.SGP.Infra
{
    public class FiltroComunicadoDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public DateTime? DataEnvioInicio { get; set; }
        public DateTime? DataEnvioFim { get; set; }
        public DateTime? DataExpiracaoInicio { get; set; }
        public DateTime? DataExpiracaoFim { get; set; }
        public string Titulo { get; set; }
        public string[] TurmasCodigo { get; set; }
        public string[] AnosEscolares { get; set; }
        public int[] TiposEscolas { get; set; }
    }
}