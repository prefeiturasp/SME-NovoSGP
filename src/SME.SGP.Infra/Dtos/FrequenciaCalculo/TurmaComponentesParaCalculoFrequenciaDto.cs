using System;

namespace SME.SGP.Infra
{
    public class TurmaComponentesParaCalculoFrequenciaDto
    {
        public TurmaComponentesParaCalculoFrequenciaDto(string turmaCodigo, string[] componentesCurricularesId, DateTime[] dataReferencia)
        {
            TurmaCodigo = turmaCodigo;
            ComponentesCurricularesId = componentesCurricularesId;
            DataReferencia = dataReferencia;
        }

        public string TurmaCodigo { get; set; }
        public string[] ComponentesCurricularesId { get; set; }
        public DateTime[] DataReferencia { get; set; }

    }
}