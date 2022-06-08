using System;

namespace SME.SGP.Infra
{
    public class FrequenciaDiariaAlunoDto
    {
        public long? Id { get; set; }
        public DateTime DataAula { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadePresenca { get; set; }
        public int QuantidadeRemoto { get; set; }
        public int QuantidadeAusencia { get; set; }
        public string Motivo { get; set; }
    }
}
