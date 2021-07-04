using SME.SGP.Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroComunicadoDto
    {
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Titulo { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public string[] Turmas { get; set; }
        public long EventoId { get; set; }

    }
}