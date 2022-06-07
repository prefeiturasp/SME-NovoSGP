using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.FechamentoNota
{
    public class WfAprovacaoNotaFechamentoTurmaDto
    {
        public WfAprovacaoNotaFechamento WfAprovacao { get; set; }
        public long TurmaId { get; set; }
        public long FechamentoTurmaDisciplinaId { get; set; }
        public ComponenteCurricular ComponenteCurricular { get; set; }
        public string CodigoAluno { get; set; }
        public double? NotaAnterior { get; set; }
        public long? ConceitoAnteriorId { get; set; }
        public int? Bimestre { get; set; }
    }
}
