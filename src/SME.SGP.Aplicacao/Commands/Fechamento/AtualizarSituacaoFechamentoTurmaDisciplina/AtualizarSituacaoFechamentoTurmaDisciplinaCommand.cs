using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AtualizarSituacaoFechamentoTurmaDisciplinaCommand : IRequest<bool>
    {
        public AtualizarSituacaoFechamentoTurmaDisciplinaCommand(long fechamentoTurmaDisciplinaId, Dominio.SituacaoFechamento situacaoFechamento)
        {
            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId;
            SituacaoFechamento = situacaoFechamento;
        }

        public long FechamentoTurmaDisciplinaId { get; }
        public SituacaoFechamento SituacaoFechamento { get; }
    }
}
