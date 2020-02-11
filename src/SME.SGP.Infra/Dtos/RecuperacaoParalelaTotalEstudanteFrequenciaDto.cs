using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaTotalEstudanteFrequenciaDto
    {
        public string FrequenciaDescricao { get; set; }
        public IEnumerable<RecuperacaoParalelaResumoFrequenciaDto> Linhas { get; set; }
        public double PorcentagemTotalFrequencia { get; set; }
        public int QuantidadeTotalFrequencia { get; set; }
    }
}