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
    public class Ao_excluir_diario_bordo_sem_devolutiva : DiarioBordoTesteBase
    {

        public Ao_excluir_diario_bordo_sem_devolutiva(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Diario de Bordo sem Devolutiva - Deve marcar registro Diário de Bordo como excluido ao efetuar a exclusão")]
        public async Task ExcluirDiarioBordoSemDevolutiva()
        {
            var filtro = new FiltroDiarioBordoDto { ContemDevolutiva = false };
            await ExecutarTeste(filtro);
        }

        public async Task ExecutarTeste(FiltroDiarioBordoDto filtro)
        {
            var useCase = ObterServicoExcluirDiarioBordoUseCase();

            var obterResgistros = ObterTodos<Dominio.DiarioBordo>();
            obterResgistros.Count.ShouldBeEquivalentTo(0);

            await CriarDadosBasicos(filtro);

            var obterResgistrosNaoExcluidos = ObterTodos<Dominio.DiarioBordo>().Where(diariobordo => !diariobordo.Excluido).ToList();
            obterResgistrosNaoExcluidos.Count.ShouldBeEquivalentTo(1);

            var excluiu = await useCase.Executar(DIARIO_BORDO_ID_1);
            excluiu.ShouldBeTrue();

            var obterResgistrosExcluidos = ObterTodos<Dominio.DiarioBordo>().Where(diariobordo => diariobordo.Excluido).ToList();
            obterResgistrosExcluidos.Count.ShouldBeEquivalentTo(1);

            obterResgistrosExcluidos.FirstOrDefault()?.Id.ShouldBeEquivalentTo(DIARIO_BORDO_ID_1);
            
            if (filtro.ContemDevolutiva)
            {
                var devolutiva = ObterTodos<Dominio.Devolutiva>().Where(devolutiva => devolutiva.Id == obterResgistrosExcluidos.FirstOrDefault()?.DevolutivaId).FirstOrDefault();
                devolutiva.ShouldNotBeNull();
                devolutiva.Excluido.ShouldBe(false);
            }

        }
    }
}