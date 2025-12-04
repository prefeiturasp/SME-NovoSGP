using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class
        ObterFuncionariosDrePerfisQueryHandlerFake : IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>
    {
        
        public ObterFuncionariosDrePerfisQueryHandlerFake()
        {}

        public async Task<IEnumerable<FuncionarioUnidadeDto>> Handle(ObterFuncionariosDreOuUePorPerfisQuery request,
            CancellationToken cancellationToken)
        => await Task.Run(() => new List<FuncionarioUnidadeDto>()
            {
                new FuncionarioUnidadeDto()
                {
                    Login = "00004",
                    NomeServidor = "Usuário Coordenador NAAPA",
                    Perfil = Perfis.PERFIL_COORDENADOR_NAAPA
                }
            });
    }
}