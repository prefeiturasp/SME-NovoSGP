using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AlunoNotaFechamentoDto
    {
        public string CodigoAluno { get; set; }
        public int? NumeroAlunoChamada { get; set; }
        public string NomeAluno { get; set; }
        public string AlteradoPor { get; set; }
        public string CriadoPor { get; set; }
        public long? ConceitoId { get; set; }
        public string ComponenteCurricularDescricao { get; set; }
        public double? NotaAnterior { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoAnteriorId { get; set; }
        public string AlteradoRf { get; set; }
        public string CriadoRf { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
