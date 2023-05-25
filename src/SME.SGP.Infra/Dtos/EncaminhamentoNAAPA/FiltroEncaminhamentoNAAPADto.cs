using System;

namespace SME.SGP.Infra
{
    public class FiltroEncaminhamentoNAAPADto
    {
        public bool ExibirHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string NomeAluno { get; set; }
        public DateTime? DataAberturaQueixaInicio { get; set; }
        public DateTime? DataAberturaQueixaFim { get; set; }
        public int Situacao { get; set; }
        public int Prioridade { get; set; }
        public bool ExibirEncerrados { get; set; }
    }
}