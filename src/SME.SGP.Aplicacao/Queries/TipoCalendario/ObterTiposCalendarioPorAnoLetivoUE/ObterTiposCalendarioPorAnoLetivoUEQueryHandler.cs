using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoUEQueryHandler : IRequestHandler<ObterTiposCalendarioPorAnoLetivoUEQuery, IEnumerable<TipoCalendario>>
    {
        private IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterTiposCalendarioPorAnoLetivoUEQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<IEnumerable<TipoCalendario>> Handle(ObterTiposCalendarioPorAnoLetivoUEQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ListarPorAnoLetivoUE(request.AnoLetivo, request.CodigoUE);
        }
    }
}
