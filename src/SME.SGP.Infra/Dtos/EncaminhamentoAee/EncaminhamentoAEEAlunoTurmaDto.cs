using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentoAEEAlunoTurmaDto
    {
        public long Id { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public string TurmaCodigo { get; set; }
        public string TurmaNome { get; set; }
        public int TurmaAno { get; set; }
        public Modalidade TurmaModalidade { get; set; }
        public SituacaoAEE Situacao { get; set; }
        public string Responsavel { get; set; }
    }
}
