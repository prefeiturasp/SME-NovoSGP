using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioPorBuscaQueryHandler : IRequestHandler<ObterTipoCalendarioPorBuscaQuery, IEnumerable<TipoCalendarioBuscaDto>>
    {
        private IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ObterTipoCalendarioPorBuscaQueryHandler(IRepositorioTipoCalendarioConsulta repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }
        public async Task<IEnumerable<TipoCalendarioBuscaDto>> Handle(ObterTipoCalendarioPorBuscaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoCalendario.ObterTiposCalendarioPorDescricaoAsync(request.Descricao);
        }
    }
}
