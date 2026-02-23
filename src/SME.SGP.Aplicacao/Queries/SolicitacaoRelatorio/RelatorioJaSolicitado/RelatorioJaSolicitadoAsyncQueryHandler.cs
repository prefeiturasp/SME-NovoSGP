using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.SolicitacaoRelatorio.RelatorioJaSolicitado
{
    public class RelatorioJaSolicitadoAsyncQueryHandler : IRequestHandler<RelatorioJaSolicitadoAsyncQuery, bool>
    {
        private readonly IRepositorioSolicitacaoRelatorio _repositorio;

        public RelatorioJaSolicitadoAsyncQueryHandler(IRepositorioSolicitacaoRelatorio repositorio)
        {
            this._repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(RelatorioJaSolicitadoAsyncQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.RelatorioJaSolicitadoAsync(request.Filtros,request.TipoRelatorio,request.UsuarioQueSolicitou);
        }
    }
}
