using System;

namespace SME.SGP.Infra
{
    public class AvaliacaoNotaAlunoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public double? NotaConceito { get; set; }
        public bool Ausente { get; set; }
        public bool Regencia { get; set; }
        public string[] Disciplinas { get; set; }
        public bool EhInterdisciplinar { get; set; }
    }
}
