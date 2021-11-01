using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoFrequenciaJustificativaDto
    {
        public IEnumerable<int> AlunosCodigo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public string Bimestre { get; set; }
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }
    }
}
