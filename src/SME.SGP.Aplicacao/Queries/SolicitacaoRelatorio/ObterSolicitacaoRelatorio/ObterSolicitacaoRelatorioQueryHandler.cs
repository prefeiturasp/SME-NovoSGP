using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSolicitacaoRelatorioQueryHandler : IRequestHandler<ObterSolicitacaoRelatorioQuery, IEnumerable<SolicitacaoRelatorio>>
    {
        private readonly IRepositorioSolicitacaoRelatorio _repositorio;

        public ObterSolicitacaoRelatorioQueryHandler(IRepositorioSolicitacaoRelatorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<SolicitacaoRelatorio>> Handle(ObterSolicitacaoRelatorioQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.ObterSolicitacaoRelatorioAsync(request.TipoRelatorio, request.ExtensaoRelatorio, request.UsuarioQueSolicitou);
        }
    }
}
