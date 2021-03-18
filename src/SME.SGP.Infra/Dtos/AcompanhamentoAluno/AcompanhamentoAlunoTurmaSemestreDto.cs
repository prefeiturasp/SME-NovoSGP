using System;

namespace SME.SGP.Infra
{
    public class AcompanhamentoAlunoTurmaSemestreDto
    {
        public long AcompanhamentoAlunoId { get; set; }
        public long AcompanhamentoAlunoSemestreId { get; set; }
        public long TurmaId { get; set; }
        public int Semestre { get; set; }
        public string AlunoCodigo { get; set; }
        public string Observacoes { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public bool PodeEditar { get; set; }


    }
}
