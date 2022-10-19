using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.DiariosBordo
{
    public class Ao_excluir_diario_bordo_com_observacoes : DiarioBordoTesteBase
    {

        public Ao_excluir_diario_bordo_com_observacoes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Diario de Bordo - Deve marcar registro de observações Diário de Bordo como excluido ao efetuar a exclusão")]
        public async Task ExcluirDiarioBordoComObservacoes()
        {
            var filtro = new FiltroDiarioBordoDto { ContemObservacoes = true };

            var useCase = ObterServicoExcluirDiarioBordoUseCase();

            var diariosBordo = ObterTodos<Dominio.DiarioBordo>();
            diariosBordo.Count.ShouldBeEquivalentTo(0);

            var diariosBordoObs = ObterTodos<Dominio.DiarioBordoObservacao>();
            diariosBordoObs.Count.ShouldBeEquivalentTo(0);

            await CriarDadosBasicos(filtro);

            var diariosBordoNaoExcluidos = ObterTodos<Dominio.DiarioBordo>().Where(diariobordo => !diariobordo.Excluido).ToList();
            diariosBordoNaoExcluidos.Count.ShouldBeEquivalentTo(1);

            var diariosBordoObsNaoExcluidos = ObterTodos<Dominio.DiarioBordoObservacao>().Where(diariobordoObs => !diariobordoObs.Excluido).ToList();
            diariosBordoObsNaoExcluidos.Count.ShouldBeEquivalentTo(2);

            var excluiu = await useCase.Executar(DIARIO_BORDO_ID_1);
            excluiu.ShouldBeTrue();

            var diariosBordoExcluidos = ObterTodos<Dominio.DiarioBordo>().Where(diariobordo => diariobordo.Excluido).ToList();
            diariosBordoExcluidos.Count.ShouldBeEquivalentTo(1);

            var diariosBordoObsExcluidos = ObterTodos<Dominio.DiarioBordoObservacao>().Where(diariobordoObs => diariobordoObs.Excluido).ToList();
            diariosBordoObsExcluidos.Count.ShouldBeEquivalentTo(2);
            diariosBordoObsExcluidos.OrderBy(obs => obs.Id);

            diariosBordoExcluidos.FirstOrDefault()?.Id.ShouldBeEquivalentTo(DIARIO_BORDO_ID_1);
            diariosBordoObsExcluidos.FirstOrDefault()?.Id.ShouldBeEquivalentTo(DIARIO_BORDO_OBS_ID_1);
            diariosBordoObsExcluidos.LastOrDefault()?.Id.ShouldBeEquivalentTo(DIARIO_BORDO_OBS_ID_2);
        }
    }
}