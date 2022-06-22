using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaUnica
{
    public class Ao_excluir_aula_unica : AulaTeste
    {
        private DateTime dataInicio = new (DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime dataFim = new (DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_excluir_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Aula_nao_encontrada()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObterDto(RecorrenciaAula.AulaUnica);
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não foi possivél localizar a aula de id : 1");
        }

        [Fact]
        public async Task Exclui_aula_unica()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObterDto(RecorrenciaAula.AulaUnica);
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Excluido.ShouldBe(true);
        }

        [Fact]
        public async Task Aula_possui_avaliacao()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataInicio, RecorrenciaAula.AulaUnica);
            await CriarAtividadeAvaliativaFundamental(dataInicio);

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObterDto(RecorrenciaAula.AulaUnica);
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");
        }
    }
}
