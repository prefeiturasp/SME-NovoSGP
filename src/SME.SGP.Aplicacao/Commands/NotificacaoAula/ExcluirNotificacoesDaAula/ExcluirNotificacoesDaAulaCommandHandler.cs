using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacoesDaAulaCommandHandler : IRequestHandler<ExcluirNotificacoesDaAulaCommand, bool>
    {
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;

        public ExcluirNotificacoesDaAulaCommandHandler(IRepositorioNotificacaoAula repositorioNotificacaoAula)
        {
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
        }

        public async Task<bool> Handle(ExcluirNotificacoesDaAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioNotificacaoAula.Excluir(request.AulaId);
            return true;
        }
    }
}
