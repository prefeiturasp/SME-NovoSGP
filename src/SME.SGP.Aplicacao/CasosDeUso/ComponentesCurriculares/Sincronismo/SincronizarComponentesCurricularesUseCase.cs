using MediatR;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SincronizarComponentesCurricularesUseCase : AbstractUseCase, ISincronizarComponentesCurricularesUseCase
    {
        public SincronizarComponentesCurricularesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var componentesEol = await mediator.Send(new ObterComponentesCurricularesEolQuery());

            var componentesSGP = await mediator.Send(new ObterComponentesCurricularesQuery());

            var inserirComponentes = componentesEol.Where(c => !componentesSGP.Any(x => x.Codigo == c.Codigo));
            
            var atualizarComponentes = componentesEol.Where(c => componentesSGP.Any(x => x.Codigo == c.Codigo && !c.Descricao.Equals(x.DescricaoEol)));

            if (inserirComponentes.Any())
                await mediator.Send(new InserirVariosComponentesCurricularesCommand(inserirComponentes));

            foreach (var atualizar in atualizarComponentes)
                await mediator.Send(new AtualizarComponenteCurricularCommand(atualizar));

            return true;
        }
    }
}
