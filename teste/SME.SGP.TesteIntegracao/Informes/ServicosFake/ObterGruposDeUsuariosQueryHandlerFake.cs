using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Informes.ServicosFake
{
    public class ObterGruposDeUsuariosQueryHandlerFake : IRequestHandler<ObterGruposDeUsuariosQuery, IEnumerable<GruposDeUsuariosDto>>
    {
        public async Task<IEnumerable<GruposDeUsuariosDto>> Handle(ObterGruposDeUsuariosQuery request, CancellationToken cancellationToken)
        {
            return new List<GruposDeUsuariosDto>()
            {
                new GruposDeUsuariosDto()
                {
                    Id = 11,
                    Nome = "AD"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 33,
                    Nome = "ADM COTIC"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 14,
                    Nome = "ADM DRE"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 32,
                    Nome = "ADM SME"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 8,
                    Nome = "ADM UE"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 51,
                    Nome = "Área Técnica"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 47,
                    Nome = "Assistente Social"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 1,
                    Nome = "ATE"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 41,
                    Nome = "ATE Secretaria"
                },
                new GruposDeUsuariosDto()
                {
                    Id = 42,
                    Nome = "Comunicados DRE"
                },
            };
        }
    }
}
