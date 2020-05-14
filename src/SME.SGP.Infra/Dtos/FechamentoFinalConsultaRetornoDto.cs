using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FechamentoFinalConsultaRetornoDto
    {
        public FechamentoFinalConsultaRetornoDto()
        {
            Alunos = new List<FechamentoFinalConsultaRetornoAlunoDto>();
            EhSintese = false;
        }

        public IList<FechamentoFinalConsultaRetornoAlunoDto> Alunos { get; set; }
        public string AuditoriaAlteracao { get; set; }
        public string AuditoriaInclusao { get; set; }
        public bool EhNota { get; set; }
        public bool EhSintese { get; set; }
        public DateTime EventoData { get; set; }
        public double FrequenciaMedia { get; set; }
        public double NotaMedia { get; set; }
    }
}