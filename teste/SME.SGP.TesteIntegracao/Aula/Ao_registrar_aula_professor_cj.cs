using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_registrar_aula_professor_cj : AulaTeste
    {
        public Ao_registrar_aula_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_CJ()
        {
            await CarregueBase(ObtenhaPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriaAtribuicaoCJ();

            await ExecuteTesteRegistre();
        }

        [Fact]
        public async Task Professor_CJ_sem_permissao_para_cadastrar_atribuicao_encerrada()
        {
            await CarregueBase(ObtenhaPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriaAtribuicaoCJ();
            await CrieAtribuicaoEsporadica();

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObtenhaDtoAula();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Você não possui permissão para cadastrar aulas neste período");
        }

        [Fact]
        public async Task Professor_CJ_nao_pode_criar_aulas()
        {
            await CarregueBase(ObtenhaPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObtenhaDtoAula();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Você não pode criar aulas para essa Turma.");
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_data_com_evento_nao_letivo()
        {
            await CarregueBase(ObtenhaPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CrieEvento(EventoLetivo.Nao);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObtenhaDtoAula();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!");
        }
    }
}
