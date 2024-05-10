using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConciliacaoFrequenciaTurmaMensalDto
    {
        public ConciliacaoFrequenciaTurmaMensalDto(List<string> turmasDaModalidade, int mes)
        {
            TurmasDaModalidade = turmasDaModalidade;
            Mes = mes;
        }

        public List<string> TurmasDaModalidade { get; set; }
        public int Mes { get; set; }
    }
}
