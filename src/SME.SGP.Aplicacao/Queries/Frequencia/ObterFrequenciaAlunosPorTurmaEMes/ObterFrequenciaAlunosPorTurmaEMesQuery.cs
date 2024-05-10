using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorTurmaEMesQuery : IRequest<IEnumerable<RegistroFrequenciaAlunoPorTurmaEMesDto>>
    {
        public ObterFrequenciaAlunosPorTurmaEMesQuery(string turmaCodigo, int mes)
        {
            TurmaCodigo = turmaCodigo;
            Mes = mes;
        }

        public string TurmaCodigo { get; set; }
        public int Mes { get; set; }
    }
}
