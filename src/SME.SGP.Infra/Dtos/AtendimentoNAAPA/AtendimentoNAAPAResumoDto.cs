using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AtendimentoNAAPAResumoDto
    {
        public long Id { get; set; }
        public string Ue { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string NomeAluno { get; set; }
        public string CodigoAluno { get; set; }
        public DateTime? DataAberturaQueixaInicio  { get; set; }
        public string Situacao { get; set; }
        public string Prioridade { get; set; }
        public bool EhMatriculadoTurmaPAP { get; set; }
        public DateTime? DataUltimoAtendimento { get; set; }
        public string Turma { get; set; }
        public string TurmaNome { get; set; }
        public Modalidade TurmaModalidade { get; set; }
    }
}