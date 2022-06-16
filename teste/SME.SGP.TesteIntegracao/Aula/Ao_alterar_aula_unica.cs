using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using System.Linq;


namespace SME.SGP.TesteIntegracao
{
    public class Ao_alterar_aula_unica : AulaTeste
    {
        public Ao_alterar_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ja_existe_aula_criada_no_dia_para_o_componente()
        {
            await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriaAula();

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var dto = ObtenhaDtoAula();
            dto.Id = 2;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Já existe uma aula criada neste dia para este componente curricular");
        }

        [Fact]
        public async Task Nao_e_possivel_alterar_aula_fora_do_periodo()
        {
            await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false);
            await CriarPeriodoEscolarEncerrado();
            await CriaAula();

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var dto = ObtenhaDtoAula();
            dto.Id = 1;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula fora do periodo escolar");
        }

        [Fact]
        public async Task Altera_aula()
        {
            await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriaAula();

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var dto = ObtenhaDtoAula();
            dto.Id = 1;
            dto.Quantidade = 1;

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Quantidade.ShouldBe(1);
        }

        [Fact]
        public async Task Altera_aula_regente_diferente_do_atual()
        {
            await CarregueBase(ObtenhaPerfilEspecialista(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriaAula("1111111");

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var dto = ObtenhaDtoAula();
            dto.Id = 1;
            dto.Quantidade = 1;
            dto.EhRegencia = true;

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Quantidade.ShouldBe(1);
        }
    }
}
