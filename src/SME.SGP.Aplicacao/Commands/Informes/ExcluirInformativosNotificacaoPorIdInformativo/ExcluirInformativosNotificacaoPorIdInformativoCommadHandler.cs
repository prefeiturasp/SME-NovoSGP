using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirInformativosNotificacaoPorIdInformativoCommadHandler : IRequestHandler<ExcluirInformativosNotificacaoPorIdInformativoCommad, bool>
    {
        private readonly IRepositorioInformativoNotificacao repositorio;

        public ExcluirInformativosNotificacaoPorIdInformativoCommadHandler(IRepositorioInformativoNotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExcluirInformativosNotificacaoPorIdInformativoCommad request, CancellationToken cancellationToken)
        {
            return repositorio.RemoverPorInformativoIdAsync(request.InformativoId);
        }
    }
}
