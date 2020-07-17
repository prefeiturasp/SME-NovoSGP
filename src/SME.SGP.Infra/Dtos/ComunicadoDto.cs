using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class ComunicadoDto
    {
        public DateTime DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public List<GrupoComunicacaoDto> Grupos { get; set; }
        public long Id { get; set; }
        public string Titulo { get; set; }
        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public TipoComunicado TipoComunicado { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Turma { get; set; }
        public IEnumerable<ComunicadoAluno> Alunos { get; set; }
    }
}