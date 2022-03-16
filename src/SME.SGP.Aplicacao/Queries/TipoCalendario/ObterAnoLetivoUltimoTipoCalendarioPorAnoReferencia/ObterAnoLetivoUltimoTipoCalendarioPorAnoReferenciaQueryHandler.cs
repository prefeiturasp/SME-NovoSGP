using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQueryHandler : IRequestHandler<ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery, int>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<int> Handle(ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario
                .ObterAnoLetivoUltimoTipoCalendarioPorDataReferencia(request.AnoReferencia, request.ModalidadeTipoCalendario);
        }
    }
}
