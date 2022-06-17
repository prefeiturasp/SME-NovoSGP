using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_excluir_aula_unica : AulaTeste
    {
        public Ao_excluir_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Aula_nao_encontrada()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObtenhaDto();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Não foi possivél localizar a aula de id : 1");
        }

        [Fact]
        public async Task Exclui_aula_unica()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriaAula();

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObtenhaDto();
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().Excluido.ShouldBe(true);
        }

        [Fact]
        public async Task Aula_possui_avaliacao()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);
            await CriaAula();
            await CriarAtividadeAvaliativaFundamental();

            var useCase = ServiceProvider.GetService<IExcluirAulaUseCase>();
            var dto = ObtenhaDto();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            excecao.Message.ShouldBe("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");
        }

        private ExcluirAulaDto ObtenhaDto()
        {
            return new ExcluirAulaDto()
            {
                AulaId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica
            };
        }

        private async Task CriarAtividadeAvaliativaFundamental()
        {
            await InserirNaBase(new TipoAvaliacao
            {
                Id = 1,
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = "2222222",
                TurmaId = "1",
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = new DateTime(2022, 02, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = "1106",
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
        }
    }
}
