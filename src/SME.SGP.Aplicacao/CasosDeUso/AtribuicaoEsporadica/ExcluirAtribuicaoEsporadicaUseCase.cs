using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAtribuicaoEsporadicaUseCase : AbstractUseCase, IExcluirAtribuicaoEsporadicaUseCase
    {
        public ExcluirAtribuicaoEsporadicaUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(long id)
        {
            var atribuicaoEsporadica = await mediator.Send(new ObterAtribuicaoEsporadicaPorIdQuery(id)); 
            if (atribuicaoEsporadica is null)
                throw new NegocioException("Não foi possível localizar esta atribuição esporádica.");

            var checarAtribuicaoCJ = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(null, "", atribuicaoEsporadica.UeId, 0, atribuicaoEsporadica.ProfessorRf, "", null, dreCodigo: atribuicaoEsporadica.DreId,
                anoLetivo: atribuicaoEsporadica.AnoLetivo));

            if (checarAtribuicaoCJ.Any())
                throw new NegocioException("Este professor possui atribuição CJ Ativa no período informado.");

            atribuicaoEsporadica.Excluir();

            await mediator.Send(new SalvarAtribuicaoEsporadicaCommand(atribuicaoEsporadica));

            return true;
        }
    }
}
