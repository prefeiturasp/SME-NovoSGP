using System;

namespace SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA
{
    public class NovoEncaminhamentoNAAPAResumoDto
    {
        public long Id { get; set; }
        public string TipoQuestionario { get; set; }
        public string UeNome { get; set; }
        public string NomeAluno { get; set; }
        public string TurmaNome { get; set; }
        public DateTime? DataAberturaQueixaInicio { get; set; }
        public DateTime? DataUltimoAtendimento { get; set; }
        public string Situacao { get; set; }
        public bool SuspeitaViolencia { get; set; }
    }
}