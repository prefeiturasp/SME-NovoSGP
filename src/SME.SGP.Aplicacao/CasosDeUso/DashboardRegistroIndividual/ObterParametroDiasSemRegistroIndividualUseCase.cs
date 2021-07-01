using MediatR;
using SME.SGP.Dominio;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroDiasSemRegistroIndividualUseCase : AbstractUseCase, IObterParametroDiasSemRegistroIndividualUseCase
    {
        public ObterParametroDiasSemRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<int?> Executar(int anoLetivo)
        {
            var parametroExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PeriodoDeDiasSemRegistroIndividual, anoLetivo));

            if (parametroExecucao == null)
                throw new NegocioException("Não foi possível localizar a última consolidação de Informações escolares para o Ano informado");

            if (!string.IsNullOrEmpty(parametroExecucao.Valor))
                return int.Parse(parametroExecucao.Valor);

            return null;
        }
    }
}
