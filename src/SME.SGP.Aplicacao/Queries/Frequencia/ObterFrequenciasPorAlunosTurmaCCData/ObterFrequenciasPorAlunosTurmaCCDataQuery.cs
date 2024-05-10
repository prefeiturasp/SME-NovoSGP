using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPorAlunosTurmaCCDataQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciasPorAlunosTurmaCCDataQuery(string[] alunosCodigo, DateTime dataReferencia, TipoFrequenciaAluno tipoFrequencia, string turmaCodigo, string componenteCurriularId)
        {
            AlunosCodigo = alunosCodigo;
            DataReferencia = dataReferencia;
            TipoFrequencia = tipoFrequencia;
            TurmaCodigo = turmaCodigo;
            ComponenteCurriularId = componenteCurriularId;
        }

        public string[] AlunosCodigo { get; set; }
        public DateTime DataReferencia { get; set; }
        public TipoFrequenciaAluno TipoFrequencia { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurriularId { get; set; }
    }
}
