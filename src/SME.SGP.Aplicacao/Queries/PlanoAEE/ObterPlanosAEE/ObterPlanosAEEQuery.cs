using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEQuery : IRequest<PaginacaoResultadoDto<PlanoAEEResumoDto>>
    {
        public ObterPlanosAEEQuery(long dreId, long ueId, long turmaId, string alunoCodigo, SituacaoPlanoAEE? situacao)
        {
            DreId = dreId;
            UeId = ueId;
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            Situacao = situacao;
        }

        public long DreId { get; }
        public long UeId { get; }
        public long TurmaId { get; }
        public string AlunoCodigo { get; }
        public SituacaoPlanoAEE? Situacao { get; }
    }
}
