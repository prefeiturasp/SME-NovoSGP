using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardTotalAtividadesCompensacaoQueryHandler : IRequestHandler<ObterDadosDashboardTotalAtividadesCompensacaoQuery, IEnumerable<TotalCompensacaoAusenciaDto>>
    {
        private readonly IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusencia;

        public ObterDadosDashboardTotalAtividadesCompensacaoQueryHandler(IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }

        public async Task<IEnumerable<TotalCompensacaoAusenciaDto>> Handle(ObterDadosDashboardTotalAtividadesCompensacaoQuery request, CancellationToken cancellationToken)
            => await repositorioCompensacaoAusencia.ObterAtividadesCompensacaoConsolidadasPorTurmaEAno(request.Anoletivo,
                                                                                                      request.DreId,
                                                                                                      request.UeId,
                                                                                                      request.ModalidadeCodigo,
                                                                                                      request.Bimestre,
                                                                                                      request.Semestre);
    }
}
