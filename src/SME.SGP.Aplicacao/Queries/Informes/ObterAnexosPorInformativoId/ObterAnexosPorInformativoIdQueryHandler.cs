using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnexosPorInformativoIdQueryHandler : IRequestHandler<ObterAnexosPorInformativoIdQuery, IEnumerable<InformativoAnexoDto>>
    {
        private readonly IRepositorioInformativoAnexo repositorio;

        public ObterAnexosPorInformativoIdQueryHandler(IRepositorioInformativoAnexo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<InformativoAnexoDto>> Handle(ObterAnexosPorInformativoIdQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAnexosPorInformativoIdAsync(request.InformativoId);
        }
    }
}
