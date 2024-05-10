using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Abrangencia.Fake
{
    public class ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandlerFake : IRequestHandler<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery, AbrangenciaCompactaVigenteRetornoEOLDTO>
    {
        const string USUARIO_PROFESSOR_LOGIN_2222222 = "2222222";
        public async Task<AbrangenciaCompactaVigenteRetornoEOLDTO> Handle(ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new AbrangenciaCompactaVigenteRetornoEOLDTO()
            {
                IdDres = new string[] { "1" },
                IdUes = new string[] { "1" },
                IdTurmas = new string[] { "1" },
                Login = USUARIO_PROFESSOR_LOGIN_2222222,
                Abrangencia = new AbrangenciaCargoRetornoEolDTO()
                {
                    Abrangencia = Infra.Enumerados.Abrangencia.UeTurmasDisciplinas,
                    CargosId = new List<int>() { 1 }
                }
            });
        }
    }
}
