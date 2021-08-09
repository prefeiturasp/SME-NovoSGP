using System;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class ComunicadoInserirDto
    {
        public ComunicadoInserirDto()
        {
            Alunos = new List<string>();
            Turmas = new List<string>();
        }
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public long Id { get; set; }
        public string Titulo { get; set; }
        public int AnoLetivo { get; set; }
        public string SeriesResumidas { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public IEnumerable<string> Turmas { get; set; }
        public bool AlunosEspecificados { get; set; }
        public int[] Modalidades { get; set; }
        public int Semestre { get; set; }
        public IEnumerable<string> Alunos { get; set; }
        public long? TipoCalendarioId { get; set; }
        public long? EventoId { get; set; }
    }

}