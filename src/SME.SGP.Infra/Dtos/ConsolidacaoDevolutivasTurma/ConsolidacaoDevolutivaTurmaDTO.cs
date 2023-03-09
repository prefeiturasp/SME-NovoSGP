using System;

namespace SME.SGP.Infra.Dtos
{
    public class ConsolidacaoDevolutivaTurmaDTO
    {
        public string DreId { get; set; }
        public string UeId { get; set; }
        public string TurmaId { get; set; }
        public int QuantidadeEstimadaDevolutivas { get; set; }
        public int QuantidadeRegistradaDevolutivas { get; set; }
    }
    
    public class QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO
    {
        public string DreId { get; set; }
        public string UeId { get; set; }
        public string TurmaId { get; set; }
        public int QuantidadeDiarioBordoRegistrado { get; set; }
    }

    public class DevolutivaTurmaDTO
    {
        public string TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public long UeId { get; set; }
        public long Id { get; set; }
        public int AnoAtual { get; set; }
        public DevolutivaTurmaDTO()
        {
            AnoAtual = DateTime.Now.Year;
        }
    }

    public class FiltroDevolutivaTurmaDTO
    {
        public string TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public long UeId { get; set; }

        public FiltroDevolutivaTurmaDTO(string turmaId, int anoLetivo, long ueId)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            UeId = ueId;
        }
    }
}
