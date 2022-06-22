using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Avaliacao
{
    public class Ao_registrar_avaliacao_para_professor_especialista : TesteAvaliacao
    {
        private DateTime dataInicio = new DateTime(2022, 05, 02);
        private DateTime dataFim = new DateTime(2022, 07, 08);
        private const string COMPONENTE_INVALIDO = "0";

        public Ao_registrar_avaliacao_para_professor_especialista(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Registrar_avaliacao_para_professor_especialista()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObtenhaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, dataInicio);

            var retorno = await comando.Inserir(dto);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Componente_curricular_nao_encontrado()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObtenhaDto(COMPONENTE_INVALIDO, CategoriaAtividadeAvaliativa.Normal, dataInicio);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Inserir(dto));

            excecao.Message.ShouldBe("Componente curricular não encontrado no EOL.");
        }

        [Fact]
        public async Task Registrar_mais_de_um_avaliacao_no_mesmo_dia_para_professor_especialista_nao_permite()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAtividadeAvaliativaFundamental(dataInicio, TipoAvaliacaoCodigo.AvaliacaoBimestral);


        }
    }
}
