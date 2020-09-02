using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidaSeExisteTipoCalendarioPorIdQueryHandler : IRequestHandler<ValidaSeExisteTipoCalendarioPorIdQuery, bool>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ValidaSeExisteTipoCalendarioPorIdQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<bool> Handle(ValidaSeExisteTipoCalendarioPorIdQuery request, CancellationToken cancellationToken)
        {
            if (await repositorioTipoCalendario.ObterPorIdAsync(request.TipoCalendarioId) == null)
                throw new NegocioException("Não foi possível encontrar o tipo de calendário");

            return true;
        }
    }
}
