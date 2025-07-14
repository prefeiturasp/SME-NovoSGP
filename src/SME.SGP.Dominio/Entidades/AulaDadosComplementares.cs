using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class AulaDadosComplementares
    {
        public bool PossuiFrequencia { get; set; }
        public bool RegistroFrequenciaExcluido { get; set; }
        public bool PossuiPlanoAula { get; set; }
        public bool RegistroPlanoAulaExcluido { get; set; }
    }
}
