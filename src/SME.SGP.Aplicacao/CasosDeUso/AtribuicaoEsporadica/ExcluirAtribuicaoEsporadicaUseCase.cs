using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
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

            var dto = new AtribuicoesPorTurmaEProfessorDto()
            {
                UeId = atribuicaoEsporadica.UeId,
                UsuarioRf = atribuicaoEsporadica.ProfessorRf,
                DreCodigo = atribuicaoEsporadica.DreId,
                AnoLetivo = atribuicaoEsporadica.AnoLetivo
            };

            var checarAtribuicaoCJ = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(dto));

            if (checarAtribuicaoCJ.Any())
                throw new NegocioException("Este professor possui atribuição CJ Ativa no período informado.");

            atribuicaoEsporadica.Excluir();

            await mediator.Send(new SalvarAtribuicaoEsporadicaCommand(atribuicaoEsporadica));

            return true;
        }
    }
}
