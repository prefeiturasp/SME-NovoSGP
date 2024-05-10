using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarRegistroExistenteTipoCalendarioQueryHandler : IRequestHandler<VerificarRegistroExistenteTipoCalendarioQuery, bool>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorio;

        public VerificarRegistroExistenteTipoCalendarioQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendarioConsulta)
        {
            this.repositorio = repositorioTipoCalendarioConsulta;
        }

        public async Task<bool> Handle(VerificarRegistroExistenteTipoCalendarioQuery request, CancellationToken cancellationToken)
            => await repositorio.VerificarRegistroExistente(request.TipoCalendarioId, request.NomeTipoCalendario);
    }
}