using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDataCriacaoRelatorioPorCodigoQueryHandler : IRequestHandler<ObterDataCriacaoRelatorioPorCodigoQuery, DataCriacaoRelatorioDto>
    {
        private readonly IRepositorioCorrelacaoRelatorio _repositorioCorrelacaoRelatorio;

        public ObterDataCriacaoRelatorioPorCodigoQueryHandler(IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio)
        {
            _repositorioCorrelacaoRelatorio = repositorioCorrelacaoRelatorio;
        }

        public async Task<DataCriacaoRelatorioDto> Handle(ObterDataCriacaoRelatorioPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await _repositorioCorrelacaoRelatorio.ObterDataCriacaoRelatorio(request.CodigoRelatorio);
        }
    }
}
