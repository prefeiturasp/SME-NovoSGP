using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;

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
        public IEnumerable<string> Alunos { get; set; }
        public long Id { get; set; }
        public string Mensagem { get; set; }
        public string Titulo { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public IEnumerable<string> Turmas { get; set; }
        public int? Semestre { get; set; }
        public TipoComunicado TipoComunicado { get; set; }
    }
}