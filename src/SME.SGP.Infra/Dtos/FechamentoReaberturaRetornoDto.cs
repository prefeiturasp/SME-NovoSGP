using System;

namespace SME.SGP.Infra
{
    public class FechamentoReaberturaRetornoDto : AuditoriaDto
    {
        public bool[] Bimestres { get; set; }
        public int BimestresQuantidadeTotal { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public string DreCodigo { get; set; }
        public long TipoCalendarioId { get; set; }
        public string UeCodigo { get; set; }
        public string AprovadoPor { get; set; }
        public DateTime? AprovadoEm { get; set; }
        public Dominio.Aplicacao Aplicacao { get; set; }
    }
}