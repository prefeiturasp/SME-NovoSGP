using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirSolicitacaoRelatorioCommandHandler : IRequestHandler<InserirSolicitacaoRelatorioCommand, long>
    {
        private readonly IRepositorioSolicitacaoRelatorio _repositorio;

        public InserirSolicitacaoRelatorioCommandHandler(IRepositorioSolicitacaoRelatorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(InserirSolicitacaoRelatorioCommand request, CancellationToken cancellationToken)
        {
            return await _repositorio.SalvarAsync(request.SolicitacaoRelatorio);
        }
    }
}
