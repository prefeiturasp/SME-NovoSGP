using System;

namespace SME.SGP.Infra
{
    public class EventoCalendarioRetornoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string TipoEvento { get; set; }
        public DateTime DataInicio { get; set; }

        public string Descricao
        {
            get => $"{DataInicio:dd/MM/yyyy} - {Nome} ({TipoEvento})";
        }
    }
}
