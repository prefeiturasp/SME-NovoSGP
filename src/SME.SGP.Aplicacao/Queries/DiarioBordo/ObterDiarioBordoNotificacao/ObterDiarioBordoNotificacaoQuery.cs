using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoNotificacaoQuery : IRequest<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>>
    {

        public ObterDiarioBordoNotificacaoQuery(long turmaId, long? observacaoId, long diarioBordoId)
        {
            TurmaId = turmaId;
            ObservacaoId = observacaoId;
            DiarioBordoId = diarioBordoId;
        }

        public long TurmaId { get; set; }
        public long? ObservacaoId { get; set; }
        public long DiarioBordoId { get; set; }
    }
}
