using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarTiposOcorrenciaQueryHandler : IRequestHandler<ListarTiposOcorrenciaQuery, IEnumerable<OcorrenciaTipoDto>>
    {
        private readonly IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo;

        public ListarTiposOcorrenciaQueryHandler(IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo)
        {
            this.repositorioOcorrenciaTipo = repositorioOcorrenciaTipo ?? throw new System.ArgumentNullException(nameof(repositorioOcorrenciaTipo));
        }

        public async Task<IEnumerable<OcorrenciaTipoDto>> Handle(ListarTiposOcorrenciaQuery request, CancellationToken cancellationToken)
        {
            var lstTiposOcorrencias = repositorioOcorrenciaTipo.Listar();

            if (lstTiposOcorrencias != null && lstTiposOcorrencias.Any())
                return await Task.FromResult(MapearParaDto(lstTiposOcorrencias));
            else
                return await Task.FromResult(Enumerable.Empty<OcorrenciaTipoDto>());
        }

        private IEnumerable<OcorrenciaTipoDto> MapearParaDto(IEnumerable<OcorrenciaTipo> tipos)
        {
            foreach (var tipo in tipos)
            {
                yield return new OcorrenciaTipoDto()
                {
                    Id = tipo.Id,
                    Descricao = tipo.Descricao
                };
            }
        }
    }
}
