using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQueryHandler : IRequestHandler<ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery, IEnumerable<TipoCalendarioRetornoDto>>
    {
        private IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<IEnumerable<TipoCalendarioRetornoDto>> Handle(ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery request, CancellationToken cancellationToken)
        {
            var modalidadesTipoCalendario = request.Modalidades.Select(m => (int)m.ObterModalidadeTipoCalendario()).ToArray();
            return await repositorioTipoCalendario.ListarPorAnoLetivoDescricaoEModalidades(request.AnoLetivo, request.Descricao, modalidadesTipoCalendario);
        }
    }
}
