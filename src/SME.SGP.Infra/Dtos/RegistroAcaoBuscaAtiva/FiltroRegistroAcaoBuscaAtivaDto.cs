using System;

namespace SME.SGP.Infra
{
    public class FiltroRegistroAcaoBuscaAtivaDto
    {
        public bool ExibirHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public string CodigoUe { get; set; }
        public long TurmaId { get; set; }
        public string NomeAluno { get; set; }
        public DateTime? DataRegistroInicio { get; set; }
        public DateTime? DataRegistroFim { get; set; }
        public bool ExibirEncerrados { get; set; }
    }
}