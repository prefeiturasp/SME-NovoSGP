using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesNotificacaoCommandHandler : IRequestHandler<SalvarInformesNotificacaoCommand, long>
    {
        private readonly IRepositorioInformativoNotificacao repositorio;

        public SalvarInformesNotificacaoCommandHandler(IRepositorioInformativoNotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(SalvarInformesNotificacaoCommand request, CancellationToken cancellationToken)
        {
            var informativoNotificacao = new InformativoNotificacao()
            {
                InformativoId = request.InformeId,
                NotificacaoId = request.NotificacaoId
            };

            return await repositorio.SalvarAsync(informativoNotificacao);
        }
    }
}
