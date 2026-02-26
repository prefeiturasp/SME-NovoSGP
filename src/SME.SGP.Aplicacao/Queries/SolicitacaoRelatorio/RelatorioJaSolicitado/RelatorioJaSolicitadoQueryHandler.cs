using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.SolicitacaoRelatorio.RelatorioJaSolicitado
{
    public class RelatorioJaSolicitadoQueryHandler : IRequestHandler<RelatorioJaSolicitadoQuery, bool>
    {
        private readonly IRepositorioSolicitacaoRelatorio _repositorio;

        public RelatorioJaSolicitadoQueryHandler(IRepositorioSolicitacaoRelatorio repositorio)
        {
            this._repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(RelatorioJaSolicitadoQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.RelatorioJaSolicitadoAsync(request.FiltrosUsados, request.TipoRelatorio,request.UsuarioQueSolicitou);
        }
    }
}
