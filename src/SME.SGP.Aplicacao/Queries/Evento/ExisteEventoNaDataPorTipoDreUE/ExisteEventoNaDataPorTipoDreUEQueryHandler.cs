using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteEventoNaDataPorTipoDreUEQueryHandler : IRequestHandler<ExisteEventoNaDataPorTipoDreUEQuery, bool>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ExisteEventoNaDataPorTipoDreUEQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<bool> Handle(ExisteEventoNaDataPorTipoDreUEQuery request, CancellationToken cancellationToken)
            => await repositorioEvento.TemEventoNosDiasETipo(request.DataReferencia, request.DataReferencia, request.TipoEvento, request.TipoCalendarioId, request.UeCodigo, request.DreCodigo);
    }
}
