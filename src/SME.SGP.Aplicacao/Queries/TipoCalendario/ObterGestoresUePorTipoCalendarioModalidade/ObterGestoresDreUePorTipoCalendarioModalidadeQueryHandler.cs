using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGestoresDreUePorTipoCalendarioModalidadeQueryHandler : IRequestHandler<ObterGestoresDreUePorTipoCalendarioModalidadeQuery, IEnumerable<GestoresDreUePorTipoModalidadeCalendarioDto>>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterGestoresDreUePorTipoCalendarioModalidadeQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<IEnumerable<GestoresDreUePorTipoModalidadeCalendarioDto>> Handle(ObterGestoresDreUePorTipoCalendarioModalidadeQuery request, CancellationToken cancellationToken)
            => await repositorioTipoCalendario.ObterGestoresUePorTipoCalendarioModalidade(request.AnoLetivo, request.TipoCalendario);
    }
}
