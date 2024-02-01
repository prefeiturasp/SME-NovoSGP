using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformativoNotificacaoCommandHandler : IRequestHandler<SalvarInformativoNotificacaoCommand, long>
    {
        private readonly IRepositorioInformativoNotificacao repositorio;

        public SalvarInformativoNotificacaoCommandHandler(IRepositorioInformativoNotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(SalvarInformativoNotificacaoCommand request, CancellationToken cancellationToken)
        {
            var informativoNotificacao = new InformativoNotificacao()
            {
                InformativoId = request.InformativoId,
                NotificacaoId = request.NotificacaoId
            };

            return await repositorio.SalvarAsync(informativoNotificacao);
        }
    }
}
