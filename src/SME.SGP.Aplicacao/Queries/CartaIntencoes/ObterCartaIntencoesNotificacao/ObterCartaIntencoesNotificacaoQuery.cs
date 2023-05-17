using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaIntencoesNotificacaoQuery : IRequest<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>
    {
        public ObterCartaIntencoesNotificacaoQuery(long turmaId, long? observacaoId, long cartaIntencoesObservacaoId)
        {
            TurmaId = turmaId;
            ObservacaoId = observacaoId;
            CartaIntencoesObservacaoId = cartaIntencoesObservacaoId;
        }

        public long TurmaId { get; set; }
        public long? ObservacaoId { get; set; }
        public long CartaIntencoesObservacaoId { get; set; }
    }
}
