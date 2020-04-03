
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotasConceitosAlunoRetornoDto
    {
        public NotasConceitosAlunoRetornoDto()
        {
            NotasAvaliacoes = new List<NotasConceitosNotaAvaliacaoRetornoDto>();
            NotasBimestre = new List<NotaConceitoBimestreRetornoDto>();
        }

        public string Id { get; set; }
        public MarcadorFrequenciaDto Marcador { get; set; }
        public bool PodeEditar { get; set; }
        public string Nome { get; set; }
        public List<NotasConceitosNotaAvaliacaoRetornoDto> NotasAvaliacoes { get; set; }
        public List<NotaConceitoBimestreRetornoDto> NotasBimestre { get; set; }
        public int NumeroChamada { get; set; }
        public int PercentualFrequencia { get; set; }
    }
}