using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsSemPeriodoFechamentoQueryHandler : IRequestHandler<ObterUEsSemPeriodoFechamentoQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUEsSemPeriodoFechamentoQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUEsSemPeriodoFechamentoQuery request, CancellationToken cancellationToken)
        {
            var modalidades = request.ModalidadeTipoCalendario.ObterModalidadesTurma();

            return await repositorioUe.ObterUEsSemPeriodoFechamento(request.PeriodoEscolarId, request.Ano, modalidades.Cast<int>().ToArray());
        }
    }
}
