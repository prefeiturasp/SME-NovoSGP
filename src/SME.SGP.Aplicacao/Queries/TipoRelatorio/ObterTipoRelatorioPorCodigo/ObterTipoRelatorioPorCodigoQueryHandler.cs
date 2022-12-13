using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoRelatorioPorCodigoQueryHandler : IRequestHandler<ObterTipoRelatorioPorCodigoQuery, int>
    {
        private readonly IRepositorioTipoRelatorio repositorioTipoRelatorio;

        public ObterTipoRelatorioPorCodigoQueryHandler(IRepositorioTipoRelatorio repositorioTipoRelatorio)
        {
            this.repositorioTipoRelatorio = repositorioTipoRelatorio ??
                                            throw new ArgumentNullException(nameof(repositorioTipoRelatorio));
        }

        public async Task<int> Handle(ObterTipoRelatorioPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoRelatorio.ObterTipoPorCodigo(request.CodigoRelatorio);
        }
    }
}