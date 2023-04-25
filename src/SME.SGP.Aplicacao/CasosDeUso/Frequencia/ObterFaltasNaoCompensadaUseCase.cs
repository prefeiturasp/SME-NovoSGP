using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using StackExchange.Redis;

namespace SME.SGP.Aplicacao
{
    public class ObterFaltasNaoCompensadaUseCase : AbstractUseCase, IObterFaltasNaoCompensadaUseCase
    {
        public ObterFaltasNaoCompensadaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<RegistroFaltasNaoCompensadaDto>> Executar(FiltroFaltasNaoCompensadasDto param)
        {
            return await mediator.Send(new ObterAusenciaParaCompensacaoQuery(
                param.CompensacaoId, 
                param.TurmaId, 
                param.DisciplinaId, 
                param.Bimestre, 
                new List<AlunoQuantidadeCompensacaoDto> { new AlunoQuantidadeCompensacaoDto(param.CodigoAluno, param.QuantidadeCompensar) }));
        }
    }
}