using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesNiveisCargosQueryHandler : IRequestHandler<ObterNotificacoesNiveisCargosQuery, IEnumerable<NotificacoesParaTratamentoCargosNiveisDto>>
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacao;

        public ObterNotificacoesNiveisCargosQueryHandler(IRepositorioNotificacaoConsulta repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }
        public async Task<IEnumerable<NotificacoesParaTratamentoCargosNiveisDto>> Handle(ObterNotificacoesNiveisCargosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioNotificacao.ObterNotificacoesParaTratamentoCargosNiveis();
        }
    }
}
