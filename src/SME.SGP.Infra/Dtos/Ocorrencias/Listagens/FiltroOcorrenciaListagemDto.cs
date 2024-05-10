using System;

namespace SME.SGP.Infra
{
    public class FiltroOcorrenciaListagemDto
    {
        public DateTime? DataOcorrenciaInicio { get; set; }
        public DateTime? DataOcorrenciaFim { get; set; }
        public string AlunoNome { get; set; }
        public string ServidorNome { get; set; }
        public string? Titulo { get; set; }
        public long? TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int? Modalidade { get; set; }
        public int? Semestre { get; set; }
        public int? TipoOcorrencia { get; set; }
        public bool ConsideraHistorico { get; set; }
    }
}