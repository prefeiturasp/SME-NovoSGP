using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase : AbstractUseCase, IObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase
    {
        public ObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<int> Executar(int ano)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.QuantidadeImagensPercursoTurma, ano));

            if (!string.IsNullOrEmpty(parametro?.Valor))
                return int.Parse(parametro.Valor);

            return 0;
        }
    }
}
