using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConciliacaoFrequenciaTurmaPorPeriodoDto 
    {
        public ConciliacaoFrequenciaTurmaPorPeriodoDto(List<string> turmasDaModalidade, int bimestre, DateTime dataInicio, DateTime dataFim, string componenteCurricularId)
        {
            TurmasDaModalidade = turmasDaModalidade;
            Bimestre = bimestre;
            DataInicio = dataInicio;
            DataFim = dataFim;
            ComponenteCurricularId = componenteCurricularId;
        }

        public List<string> TurmasDaModalidade { get; set; }
        public int Bimestre { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string ComponenteCurricularId { get; set; }
    }
}
