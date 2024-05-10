using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesNiveisCargosQuery : IRequest<IEnumerable<NotificacoesParaTratamentoCargosNiveisDto>>
    {
        private static ObterNotificacoesNiveisCargosQuery _instance;
        public static ObterNotificacoesNiveisCargosQuery Instance => _instance ??= new();
    }
}
