using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNomeTipoCalendarioPorIdQueryHandler : IRequestHandler<ObterNomeTipoCalendarioPorIdQuery, string>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterNomeTipoCalendarioPorIdQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<string> Handle(ObterNomeTipoCalendarioPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioTipoCalendario.ObterNomePorId(request.TipoCalendarioId);
    }
}
