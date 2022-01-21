using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoFrequenciaJustificativaDto
    {
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularId { get; set; }
        public int Bimestre { get; set; }
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }
        public IEnumerable<string> AlunosCodigos { get; set; }

    }
}
