using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNomeTipoCalendarioPorIdQueryHandler : IRequestHandler<ObterNomeTipoCalendarioPorIdQuery, string>
    {
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterNomeTipoCalendarioPorIdQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<string> Handle(ObterNomeTipoCalendarioPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioTipoCalendario.ObterNomePorId(request.TipoCalendarioId);
    }
}
