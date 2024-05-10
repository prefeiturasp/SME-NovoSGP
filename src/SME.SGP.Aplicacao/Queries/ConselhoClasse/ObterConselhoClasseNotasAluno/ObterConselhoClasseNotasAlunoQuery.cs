using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseNotasAlunoQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterConselhoClasseNotasAlunoQuery(long conselhoClasseId, string alunoCodigo, int bimestre, long? componenteCurricularId = null, long? tipoCalendario = null)
        {
            ConselhoClasseId = conselhoClasseId;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            ComponenteCurricularId = componenteCurricularId;
            TipoCalendario = tipoCalendario;    
        }

        public long ConselhoClasseId { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public long? ComponenteCurricularId { get; set; }
        public long? TipoCalendario { get; set; }
    }
}
