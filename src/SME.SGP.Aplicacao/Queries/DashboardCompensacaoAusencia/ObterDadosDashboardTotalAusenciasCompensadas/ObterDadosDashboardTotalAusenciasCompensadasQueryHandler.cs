using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardTotalAusenciasCompensadasQueryHandler : IRequestHandler<ObterDadosDashboardTotalAusenciasCompensadasQuery, IEnumerable<TotalAusenciasCompensadasDto>>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ObterDadosDashboardTotalAusenciasCompensadasQueryHandler(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }

        public async Task<IEnumerable<TotalAusenciasCompensadasDto>> Handle(ObterDadosDashboardTotalAusenciasCompensadasQuery request, CancellationToken cancellationToken)
            => await repositorioCompensacaoAusencia.ObterCompesacoesAusenciasConsolidadasPorTurmaEAno(request.Anoletivo,
                                                                                                      request.DreId,
                                                                                                      request.UeId,
                                                                                                      request.ModalidadeCodigo,
                                                                                                      request.Bimestre,
                                                                                                      request.Semestre);
    }
}
