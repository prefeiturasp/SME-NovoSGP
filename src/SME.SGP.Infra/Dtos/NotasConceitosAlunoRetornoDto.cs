using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotasConceitosAlunoRetornoDto
    {
        public NotasConceitosAlunoRetornoDto()
        {
            NotasAvaliacoes = new List<NotasConceitosNotaAvaliacaoRetornoDto>();
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public List<NotasConceitosNotaAvaliacaoRetornoDto> NotasAvaliacoes { get; set; }
        public int NumeroChamada { get; set; }
    }
}