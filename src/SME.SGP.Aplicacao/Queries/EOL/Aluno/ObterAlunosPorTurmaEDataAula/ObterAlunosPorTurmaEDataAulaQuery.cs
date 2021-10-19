using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEDataAulaQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public string CodigoTurma { get; set; }
        public DateTime DataAula { get; set; }

        public ObterAlunosPorTurmaEDataAulaQuery(string codigoTurma, DateTime dataAula)
        {
            CodigoTurma = codigoTurma;
            DataAula = dataAula;
        }
    }
}
