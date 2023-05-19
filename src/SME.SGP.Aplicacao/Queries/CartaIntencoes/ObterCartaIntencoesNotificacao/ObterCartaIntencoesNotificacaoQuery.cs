using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaIntencoesNotificacaoQuery : IRequest<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>
    {
        public ObterCartaIntencoesNotificacaoQuery(long turmaId, string componenteCurricular)
        {
            TurmaId = turmaId;            
            ComponenteCurricular = componenteCurricular;
        }

        public long TurmaId { get; set; }
        public string ComponenteCurricular { get; set; }

    }
}
