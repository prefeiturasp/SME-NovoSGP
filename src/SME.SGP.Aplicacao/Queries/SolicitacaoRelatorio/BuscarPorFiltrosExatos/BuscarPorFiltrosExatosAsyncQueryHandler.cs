using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class BuscarPorFiltrosExatosAsyncQueryHandler : IRequestHandler<BuscarPorFiltrosExatosAsyncQuery, IEnumerable<SolicitacaoRelatorio>>
    {
        private readonly IRepositorioSolicitacaoRelatorio _repositorio;

        public BuscarPorFiltrosExatosAsyncQueryHandler(IRepositorioSolicitacaoRelatorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<SolicitacaoRelatorio>> Handle(BuscarPorFiltrosExatosAsyncQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.BuscarPorFiltrosExatosAsync(request.Filtros, request.TipoRelatorio, request.StatusSolicitacao);
        }
    }
}
