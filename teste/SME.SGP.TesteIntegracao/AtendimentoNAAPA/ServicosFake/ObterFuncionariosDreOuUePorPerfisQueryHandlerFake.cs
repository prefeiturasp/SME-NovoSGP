using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nest;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFakes
{
    public class ObterFuncionariosDreOuUePorPerfisQueryHandlerFake : IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>
    {
        public ObterFuncionariosDreOuUePorPerfisQueryHandlerFake()
        {}

        public async Task<IEnumerable<FuncionarioUnidadeDto>> Handle(ObterFuncionariosDreOuUePorPerfisQuery request, CancellationToken cancellationToken)
        {
            var funcionariosUnidade = new List<FuncionarioUnidadeDto>()
            {
                new FuncionarioUnidadeDto()
                {
                    Login = "0000001",
                    NomeServidor = "PERFIL COORDENADOR NAAPA",
                    Perfil = Perfis.PERFIL_COORDENADOR_NAAPA
                },
                new FuncionarioUnidadeDto()
                {
                    Login = "0000002",
                    NomeServidor = "PERFIL PSICOPEDAGOGO",
                    Perfil = Perfis.PERFIL_PSICOPEDAGOGO
                },
                new FuncionarioUnidadeDto()
                {
                    Login = "0000003",
                    NomeServidor = "PERFIL PSICOLOGO ESCOLAR",
                    Perfil = Perfis.PERFIL_PSICOLOGO_ESCOLAR
                },
                new FuncionarioUnidadeDto()
                {
                    Login = "0000004",
                    NomeServidor = "PERFIL ASSISTENTE SOCIAL",
                    Perfil = Perfis.PERFIL_ASSISTENTE_SOCIAL
                }
            };

            return funcionariosUnidade;
        }
    }

}