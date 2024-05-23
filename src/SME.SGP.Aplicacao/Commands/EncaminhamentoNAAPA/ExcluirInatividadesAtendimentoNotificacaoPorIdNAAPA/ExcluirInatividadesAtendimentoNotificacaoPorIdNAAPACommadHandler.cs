using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommandHandler : IRequestHandler<ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommand, bool>
    {
        private readonly IRepositorioInatividadeAtendimentoNAAPANotificacao repositorio;

        public ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommandHandler(IRepositorioInatividadeAtendimentoNAAPANotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExcluirInatividadesAtendimentoNotificacaoPorIdNAAPACommand request, CancellationToken cancellationToken)
        {
            if (request.ExclusaoLogica)
                return repositorio.RemoverLogicoPorNAAPAIdAsync(request.EncaminhamentoNAAPAId);
            return repositorio.RemoverPorNAAPAIdAsync(request.EncaminhamentoNAAPAId);
        }
    }
}
