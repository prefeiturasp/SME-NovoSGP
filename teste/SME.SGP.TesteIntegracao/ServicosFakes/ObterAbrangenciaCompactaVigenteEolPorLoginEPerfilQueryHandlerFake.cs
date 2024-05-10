using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dto;
using SME.SGP.Infra.Enumerados;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandlerFake : IRequestHandler<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery, AbrangenciaCompactaVigenteRetornoEOLDTO>
    {
        public async Task<AbrangenciaCompactaVigenteRetornoEOLDTO> Handle(ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            return new AbrangenciaCompactaVigenteRetornoEOLDTO
            {
                Login = request.Login,
                Abrangencia = new AbrangenciaCargoRetornoEolDTO
                {
                    GrupoID = new Guid("40e1e074-37d6-e911-abd6-f81654fe895d"),
                    CargosId = new List<int>
                    {
                        3239, 3255, 3263, 3271, 3280, 3298, 3301, 3336, 3344, 3840, 3859, 3867, 3874, 3883, 3884, 3310, 3131, 3212, 3213, 3220, 3247, 3395, 3425, 3433, 3450, 3816, 3875, 3877, 3880
                    },
                    Grupo = GruposSGP.Professor,
                    Abrangencia = Infra.Enumerados.Abrangencia.Professor,
                },
                IdTurmas = new List<string> { "2366531" }.ToArray(),
            };
        }
    }
}
