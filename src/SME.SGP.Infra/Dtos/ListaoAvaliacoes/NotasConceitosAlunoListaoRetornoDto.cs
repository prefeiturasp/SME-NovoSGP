using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotasConceitosAlunoListaoRetornoDto
    {
        public NotasConceitosAlunoListaoRetornoDto()
        {
            NotasAvaliacoes = new List<NotasConceitosNotaAvaliacaoListaoRetornoDto>();
        }

        public string Id { get; set; }
        public bool PodeEditar { get; set; }
        public string Nome { get; set; }
        public MarcadorFrequenciaDto Marcador { get; set; }
        public List<NotasConceitosNotaAvaliacaoListaoRetornoDto> NotasAvaliacoes { get; set; }
        public int NumeroChamada { get; set; }
        public bool EhAtendidoAEE { get; set; }
    }
}
