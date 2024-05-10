using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendariosPorBuscaQueryHandler : IRequestHandler<ObterTiposCalendariosPorBuscaQuery, IEnumerable<TipoCalendarioBuscaDto>>
    {
        private IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterTiposCalendariosPorBuscaQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<IEnumerable<TipoCalendarioBuscaDto>> Handle(ObterTiposCalendariosPorBuscaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ObterTiposCalendarioPorDescricaoAsync(request.Descricao);
        }
    }
}
