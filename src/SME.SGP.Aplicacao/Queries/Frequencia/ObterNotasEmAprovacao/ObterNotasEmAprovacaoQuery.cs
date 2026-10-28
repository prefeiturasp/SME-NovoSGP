using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasEmAprovacaoQuery : IRequest<IEnumerable<NotaEmAprovacaoFechamentoDto>>
    {
        public ObterNotasEmAprovacaoQuery(string[] codigosAlunos, long[] turmaFechamentoIds)
        {
            CodigosAlunos = codigosAlunos;
            TurmaFechamentoIds = turmaFechamentoIds;
        }

        public string[] CodigosAlunos { get; set; }
        public long[] TurmaFechamentoIds { get; set; }
    }
}
