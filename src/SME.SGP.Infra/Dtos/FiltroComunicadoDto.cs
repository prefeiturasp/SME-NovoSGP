using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroComunicadoDto
    {
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public int[] GruposId { get; set; }
        public string Titulo { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public string Turma { get; set; }
    }
}