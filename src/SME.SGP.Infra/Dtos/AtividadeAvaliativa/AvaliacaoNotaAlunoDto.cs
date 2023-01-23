using System;

namespace SME.SGP.Infra
{
    public class AvaliacaoNotaAlunoDto
    {
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public double? NotaConceito { get; set; }
        public bool Ausente { get; set; }
    }
}
