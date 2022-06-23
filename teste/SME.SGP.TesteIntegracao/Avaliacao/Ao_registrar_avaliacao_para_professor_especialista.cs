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

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public class Ao_registrar_avaliacao_para_professor_especialista : TesteAvaliacao
    {
        private DateTime dataInicio = new DateTime(2022, 05, 02);
        private DateTime dataFim = new DateTime(2022, 07, 08);
        
        public Ao_registrar_avaliacao_para_professor_especialista(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Registrar_avaliacao_para_professor_especialista()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObterAtividadeAvaliativaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, dataInicio, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            var retorno = await comando.Inserir(dto);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task Componente_curricular_nao_encontrado()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();
            var dto = ObterAtividadeAvaliativaDto(COMPONENTE_INVALIDO, CategoriaAtividadeAvaliativa.Normal, dataInicio, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.Inserir(dto));

            excecao.Message.ShouldBe("Componente curricular não encontrado no EOL.");
        }

        public async Task Registrar_avaliacao_para_professor_especialista_multidiciplinar()
        {
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto()
        {
            return new CriacaoDeDadosDto()
            {
                Perfil = ObterPerfilProfessor(),
                ModalidadeTurma = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoCalendarioId = 1,
                DataInicio = dataInicio,
                DataFim = dataFim,
                TipoAvaliacao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                Bimestre = BIMESTRE_2
            };
        }
    }
}
