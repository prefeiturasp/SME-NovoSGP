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
        private IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<IEnumerable<TipoCalendarioRetornoDto>> Handle(ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<int> modalidadesTipoCalendario;

            if (request.Modalidades.Any(c => c == -99))
            {
                modalidadesTipoCalendario = request.Modalidades;
            }
            else
            {
                var modalidades = request.Modalidades.Select(m => (Modalidade)m);

                modalidadesTipoCalendario = modalidades.Select(m => (int)m.ObterModalidadeTipoCalendario()).Distinct().ToArray();
            }            

            return await repositorioTipoCalendario.ListarPorAnoLetivoDescricaoEModalidades(request.AnoLetivo, request.Descricao, modalidadesTipoCalendario);
        }
    }
}
