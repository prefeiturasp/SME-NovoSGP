using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroAcompanhamentoFrequenciaDto
    {
        public FiltroAcompanhamentoFrequenciaDto()
        {
            CodigoCriancasSelecionadas = new List<int>();
        }
        public bool TodasCrianca { get; set; }
        public List<int> CodigoCriancasSelecionadas { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Bimestre { get; set; }
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }
    }
}
