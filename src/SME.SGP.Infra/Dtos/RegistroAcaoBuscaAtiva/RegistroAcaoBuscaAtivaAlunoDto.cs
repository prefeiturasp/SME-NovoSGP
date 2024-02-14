using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroAcaoBuscaAtivaAlunoDto
    {
        public RegistroAcaoBuscaAtivaAlunoDto()
        {}
        public long Id { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public string TurmaCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }
        public string AnoTurma { get; set; }
        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataBuscaAtiva { get; set; }
        public List<RegistroAcaoBuscaAtivaSecaoDto> Secoes { get; set; }
    }
}
