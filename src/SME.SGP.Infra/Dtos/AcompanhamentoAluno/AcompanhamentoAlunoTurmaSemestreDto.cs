using System;

namespace SME.SGP.Infra
{
    public class AcompanhamentoAlunoTurmaSemestreDto
    {
        public long AcompanhamentoAlunoId { get; set; }
        public long AcompanhamentoAlunoSemestreId { get; set; }
        public string Observacoes { get; set; }
        public string PercursoIndividual { get; set; }
        public int QuantidadeFotos { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public bool PodeEditar { get; set; }
        public bool TextoSugerido { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
