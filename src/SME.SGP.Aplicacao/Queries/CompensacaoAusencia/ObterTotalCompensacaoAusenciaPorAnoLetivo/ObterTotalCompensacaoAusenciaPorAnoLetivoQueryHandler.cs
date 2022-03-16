using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacaoAusenciaPorAnoLetivoQueryHandler : IRequestHandler<ObterTotalCompensacaoAusenciaPorAnoLetivoQuery, TotalCompensacaoAusenciaDto>
    {
        private readonly IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusencia;

        public ObterTotalCompensacaoAusenciaPorAnoLetivoQueryHandler(IRepositorioCompensacaoAusenciaConsulta repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }

        public async Task<TotalCompensacaoAusenciaDto> Handle(ObterTotalCompensacaoAusenciaPorAnoLetivoQuery request, CancellationToken cancellationToken)
            => await repositorioCompensacaoAusencia.ObterTotalPorAno(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade, request.Semestre, request.Bimestre);
    }
}
