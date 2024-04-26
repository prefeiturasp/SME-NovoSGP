using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase : AbstractUseCase, IObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase
    {
        public ObterAlunosSinalizadosPrioridadeMapeamentoEstudanteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<AlunoSinalizadoPrioridadeMapeamentoEstudanteDto[]> Executar(long turmaId, int bimestre)
        {
            return mediator.Send(new ObterCodigosAlunosSinalizadosPrioridadeMapeamentoEstudanteQuery(turmaId, bimestre));   
        }
    }
}
