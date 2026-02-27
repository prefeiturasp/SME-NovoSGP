using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    internal class FinalizarSolicitacaoRelatorioCommandHandler : IRequestHandler<FinalizarSolicitacaoRelatorioCommand, long>
    {
        private readonly IRepositorioSolicitacaoRelatorio _repositorio;

        public FinalizarSolicitacaoRelatorioCommandHandler(IRepositorioSolicitacaoRelatorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<long> Handle(FinalizarSolicitacaoRelatorioCommand request, CancellationToken cancellationToken)
        {
            request.SolicitacaoRelatorio.StatusSolicitacao = Dominio.Enumerados.StatusSolicitacao.Entregue;
            return await _repositorio.SalvarAsync(request.SolicitacaoRelatorio);
        }
    }
}


