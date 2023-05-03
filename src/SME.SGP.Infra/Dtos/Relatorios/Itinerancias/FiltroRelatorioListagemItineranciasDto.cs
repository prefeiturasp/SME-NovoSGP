using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioListagemItineranciasDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int[] SituacaoIds { get; set; }
        public string[] CodigosPAAIResponsavel { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}
