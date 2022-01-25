using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequeciaPorDataPeriodoDto
    {
        public FrequeciaPorDataPeriodoDto()
        {
            Aulas = new List<FrequenciaAulaPorDataPeriodoDto>();
            AulasDetalhes = new List<FrequenciaDetalhadaDto>();
        }
        public string CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public IndicativoFrequenciaDto IndicativoFrequencia { get; set; }
        public IList<FrequenciaAulaPorDataPeriodoDto> Aulas { get; set; }
        public IList<FrequenciaDetalhadaDto> AulasDetalhes { get; set; }
    }
}
