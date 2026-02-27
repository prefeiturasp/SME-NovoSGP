using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSolicitacaoRelatorioPorIdQueryHandler : IRequestHandler<ObterSolicitacaoRelatorioPorIdQuery, SolicitacaoRelatorio>
    {
        private readonly IRepositorioSolicitacaoRelatorio _repositorio;

        public ObterSolicitacaoRelatorioPorIdQueryHandler(IRepositorioSolicitacaoRelatorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<SolicitacaoRelatorio> Handle(ObterSolicitacaoRelatorioPorIdQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.ObterPorIdAsync(request.SolicitacaoRelatorioId);
        }
    }
}


