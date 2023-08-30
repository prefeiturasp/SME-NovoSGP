using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nest;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.EncaminhamentoAEE.ServicosFake
{
    public class ObterFuncionariosDreOuUePorPerfisQueryHandlerFake : IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>
    {
        public ObterFuncionariosDreOuUePorPerfisQueryHandlerFake()
        {}

        public async Task<IEnumerable<FuncionarioUnidadeDto>> Handle(ObterFuncionariosDreOuUePorPerfisQuery request, CancellationToken cancellationToken)
        {
            var funcionariosUnidade = new List<FuncionarioUnidadeDto>()
            {
                
            };

            return funcionariosUnidade;
        }
    }

}