using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPAResumoDto
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
    }
}