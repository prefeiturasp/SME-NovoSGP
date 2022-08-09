using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class ObterNotasFechamentosPorTurmasIdsQuery : IRequest<IEnumerable<NotaConceitoComponenteBimestreAlunoDto>>
{
    public long[] TurmasIds { get; }

    public ObterNotasFechamentosPorTurmasIdsQuery(long[] turmasIds)
    {
        TurmasIds = turmasIds;
    }
}