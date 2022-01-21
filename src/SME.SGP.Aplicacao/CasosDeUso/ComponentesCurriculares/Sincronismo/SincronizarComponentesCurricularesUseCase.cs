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
            var componentesSGP = await mediator.Send(new ObterComponentesCurricularesSgpQuery());
            var inserirComponentes = componentesEol.Where(c => !componentesSGP.Any(x => x.Id == int.Parse(c.Codigo)));
            var atualizarComponentes = componentesSGP.Where(x => componentesEol.Any(c => int.Parse(c.Codigo) == x.Id && !c.Descricao.Equals(x.DescricaoSGP)));

            if (inserirComponentes.Any())
                await mediator.Send(new InserirVariosComponentesCurricularesCommand(inserirComponentes));

            if (atualizarComponentes.Any())
                await mediator.Send(new AtualizarVariosComponentesCurricularesCommand(atualizarComponentes));

            return true;
        }
    }
}
