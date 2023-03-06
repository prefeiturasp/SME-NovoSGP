using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AvaliacaoAula
{
    public class Ao_registrar_avaliacao_para_professor_regente : TesteAvaliacao
    {
        
        public Ao_registrar_avaliacao_para_professor_regente(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaTrue), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_permitir_Registrar_avaliacao_para_professor_regente_fundamental()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TIPO_CALENDARIO_1);

            string[] disciplinaRegencia = { COMPONENTE_CIENCIAS_ID_89, COMPONENTE_GEOGRAFIA_ID_8, COMPONENTE_HISTORIA_ID_7, COMPONENTE_LINGUA_PORTUGUESA_ID_138 };

            var atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia);

            await ValidarInsercaoAvaliacao(atividadeAvaliativa, NUMERO_1, RETORNAR_4, NUMERO_1, true);

        }

        [Fact]
        public async Task Deve_permitir_registrar_avaliacao_para_professor_regente_eja()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), TIPO_CALENDARIO_1);

            string[] disciplinaRegencia = { COMPONENTE_CIENCIAS_ID_89, COMPONENTE_GEOGRAFIA_ID_8, COMPONENTE_HISTORIA_ID_7, COMPONENTE_LINGUA_PORTUGUESA_ID_138 };

            var atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia);

            await ValidarInsercaoAvaliacao(atividadeAvaliativa, NUMERO_1, RETORNAR_4, NUMERO_1,true);

        }

        [Fact]
        public async Task Deve_permitir_registrar_mais_de_uma_avaliacao_para_professor_regente()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), TIPO_CALENDARIO_1);

            string[] disciplinaRegencia = { COMPONENTE_CIENCIAS_ID_89, COMPONENTE_GEOGRAFIA_ID_8 };

            var atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia);

            await ValidarInsercaoAvaliacao(atividadeAvaliativa, NUMERO_1, NUMERO_2, NUMERO_1,true);

            disciplinaRegencia = new string[] { COMPONENTE_LINGUA_PORTUGUESA_ID_138 };

            atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia, NOME_ATIVIDADE_AVALIATIVA_2);

            await ValidarInsercaoAvaliacao(atividadeAvaliativa, NUMERO_2, NUMERO_3, NUMERO_2, true);

        }

        [Fact]
        public async Task Nao_deve_permitir_registrar_mais_de_uma_avaliacao_para_professor_especialista()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), TIPO_CALENDARIO_1);

            string[] disciplinaRegencia = { COMPONENTE_CIENCIAS_ID_89, COMPONENTE_GEOGRAFIA_ID_8 };

            var atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia);

            await ValidarInsercaoAvaliacao(atividadeAvaliativa, NUMERO_1, NUMERO_2, NUMERO_1, true);

            disciplinaRegencia = new string[] { COMPONENTE_LINGUA_PORTUGUESA_ID_138 };

            atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia, NOME_ATIVIDADE_AVALIATIVA_2);

            await ValidarInsercaoAvaliacao(atividadeAvaliativa, NUMERO_2, NUMERO_3, NUMERO_2, true);

        }

        [Fact]
        public async Task Deve_permitir_copiar_avaliacao_para_uma_outra_turma_para_professor_especialista()
        {
            var dadosCriacao = ObterCriacaoDeDadosDto();

            await CriarDadosBasicos(dadosCriacao);

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            var atividadeAvaliativa = ObterAtividadeAvaliativaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral);

            await CriarTurma(dadosCriacao.ModalidadeTurma, ANO_2, TURMA_CODIGO_2);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_2, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);

            await ValidarInsercaoAvaliacao(atividadeAvaliativa, NUMERO_1, NUMERO_0, NUMERO_1);

            atividadeAvaliativa.TurmasParaCopiar = new List<CopiarAtividadeAvaliativaDto>() { new CopiarAtividadeAvaliativaDto() { TurmaId = TURMA_CODIGO_2, DataAtividadeAvaliativa = DATA_24_01}};

            await ValidarAtualizacaoAvaliacao(atividadeAvaliativa, NUMERO_2, NUMERO_0, NUMERO_2, NUMERO_1);
        }

        private async Task ValidarInsercaoAvaliacao(AtividadeAvaliativaDto atividadeAvaliativa, int qtdeAtividadeAvaliativa, int qtdeAtividadeRegencia, int qtdeAtividadeDisciplina, bool ehRegencia = false)
        {
            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa);

            var retorno = await Inserir(atividadeAvaliativa, filtroAtividadeAvaliativa);

            retorno.ShouldNotBeNull();

            ValidarAvaliacoesCadastradas(qtdeAtividadeAvaliativa, qtdeAtividadeRegencia, qtdeAtividadeDisciplina, ehRegencia);
        }

        private async Task ValidarAtualizacaoAvaliacao(AtividadeAvaliativaDto atividadeAvaliativa, int qtdeAtividadeAvaliativa, int qtdeAtividadeRegencia, int qtdeAtividadeDisciplina, int avaliacaoId)
        {
            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa,avaliacaoId);

            var retorno = await Alterar(atividadeAvaliativa, filtroAtividadeAvaliativa, avaliacaoId);

            retorno.ShouldNotBeNull();

            ValidarAvaliacoesCadastradas(qtdeAtividadeAvaliativa, qtdeAtividadeRegencia, qtdeAtividadeDisciplina);
        }

        private void ValidarAvaliacoesCadastradas(int qtdeAtividadeAvaliativa, int qtdeAtividadeRegencia, int qtdeAtividadeDisciplina, bool ehRegencia = false)
        {
            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();

            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(qtdeAtividadeAvaliativa);

            if (ehRegencia)
            {
                var atividadeAvaliativasRegencia = ObterTodos<AtividadeAvaliativaRegencia>();

                atividadeAvaliativasRegencia.ShouldNotBeEmpty();

                atividadeAvaliativasRegencia.Count().ShouldBeEquivalentTo(qtdeAtividadeRegencia);
            }

            var atividadeAvaliativasDisciplina = ObterTodos<AtividadeAvaliativaDisciplina>();

            atividadeAvaliativasDisciplina.ShouldNotBeEmpty();

            atividadeAvaliativasDisciplina.Count().ShouldBeEquivalentTo(qtdeAtividadeDisciplina);
        }

        private async Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> Inserir(AtividadeAvaliativaDto atividadeAvaliativa, FiltroAtividadeAvaliativaDto filtroAtividadeAvaliativa)
        {
            var comando = await ValidacaoBasica(filtroAtividadeAvaliativa);

            var retorno = await comando.Inserir(atividadeAvaliativa);

            return retorno;
        }

        private async Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> Alterar(AtividadeAvaliativaDto atividadeAvaliativa, FiltroAtividadeAvaliativaDto filtroAtividadeAvaliativa, int avaliacaoId)
        {
            var comando = await ValidacaoBasica(filtroAtividadeAvaliativa);

            var retorno = await comando.Alterar(atividadeAvaliativa, long.Parse(avaliacaoId.ToString()));

            return retorno;
        }

        private async Task<IComandosAtividadeAvaliativa> ValidacaoBasica(FiltroAtividadeAvaliativaDto filtroAtividadeAvaliativa)
        {
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await comando.Validar(filtroAtividadeAvaliativa).ShouldNotThrowAsync();

            return comando;
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto()
        {
            return new CriacaoDeDadosDto()
            {
                Perfil = ObterPerfilProfessor(),
                ModalidadeTurma = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                DataInicio = DATA_03_01,
                DataFim = DATA_28_04,
                TipoAvaliacao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                Bimestre = BIMESTRE_1
            };
        }
    }
}
