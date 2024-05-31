using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Informe.ServicosFake
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
                    Nome = "AD",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 33,
                    Nome = "ADM COTIC",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 14,
                    Nome = "ADM DRE",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 32,
                    Nome = "ADM SME",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 8,
                    Nome = "ADM UE",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 51,
                    Nome = "Área Técnica",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 47,
                    Nome = "Assistente Social",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 1,
                    Nome = "ATE",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 41,
                    Nome = "ATE Secretaria",
                    GuidPerfil = Guid.NewGuid()
                },
                new GruposDeUsuariosDto()
                {
                    Id = 42,
                    Nome = "Comunicados DRE",
                    GuidPerfil = Guid.NewGuid()
                }
            };
        }
    }
}
