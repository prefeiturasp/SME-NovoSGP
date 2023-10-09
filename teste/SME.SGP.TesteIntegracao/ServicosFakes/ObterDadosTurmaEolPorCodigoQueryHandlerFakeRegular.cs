using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular : IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>
    {
        public ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular(){}

        public async Task<DadosTurmaEolDto> Handle(ObterDadosTurmaEolPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return new DadosTurmaEolDto { TipoTurma = TipoTurma.Regular};
        }
    }
}