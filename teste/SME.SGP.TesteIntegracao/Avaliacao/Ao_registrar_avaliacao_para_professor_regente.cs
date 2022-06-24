using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public class Ao_registrar_avaliacao_para_professor_regente : TesteAvaliacao
    {
        private readonly DateTime DATA_24_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 24);

        private const long TIPO_CALENDARIO_1 = 1;

        private const int RETORNAR_1 = 1;
        private const int RETORNAR_2 = 2;
        private const int RETORNAR_3 = 3;
        private const int RETORNAR_4 = 4;

        public Ao_registrar_avaliacao_para_professor_regente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_permitir_Registrar_avaliacao_para_professor_regente_fundamental()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TIPO_CALENDARIO_1);

            string[] disciplinaRegencia = { COMPONENTE_CIENCIAS_ID_89, COMPONENTE_GEOGRAFIA_ID_8, COMPONENTE_HISTORIA_ID_7, COMPONENTE_LINGUA_PORTUGUESA_ID_138 };

            var atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia);

            await ValidarRetornoTeste(atividadeAvaliativa, RETORNAR_1, RETORNAR_4, RETORNAR_1);

        }

        [Fact]
        public async Task Deve_permitir_registrar_avaliacao_para_professor_regente_eja()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), TIPO_CALENDARIO_1);

            string[] disciplinaRegencia = { COMPONENTE_CIENCIAS_ID_89, COMPONENTE_GEOGRAFIA_ID_8, COMPONENTE_HISTORIA_ID_7, COMPONENTE_LINGUA_PORTUGUESA_ID_138 };

            var atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia);

            await ValidarRetornoTeste(atividadeAvaliativa, RETORNAR_1, RETORNAR_4, RETORNAR_1);

        }

        [Fact]
        public async Task Deve_permitir_registrar_mais_de_uma_avaliacao_para_professor_regente()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), TIPO_CALENDARIO_1);

            string[] disciplinaRegencia = { COMPONENTE_CIENCIAS_ID_89, COMPONENTE_GEOGRAFIA_ID_8 };

            var atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia);

            await ValidarRetornoTeste(atividadeAvaliativa, RETORNAR_1, RETORNAR_2, RETORNAR_1);

            disciplinaRegencia = new string[] { COMPONENTE_LINGUA_PORTUGUESA_ID_138 };

            atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia, NOME_ATIVIDADE_AVALIATIVA_2);

            await ValidarRetornoTeste(atividadeAvaliativa, RETORNAR_2, RETORNAR_3, RETORNAR_2);

        }

        private async Task ValidarRetornoTeste(AtividadeAvaliativaDto atividadeAvaliativa, int qtdeAtividadeAvaliativa, int qtdeAtividadeRegencia, int qtdeAtividadeDisciplina)
        {
            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa);

            var retorno = await ValidarEInserir(atividadeAvaliativa, filtroAtividadeAvaliativa);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();

            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(qtdeAtividadeAvaliativa);

            var atividadeAvaliativasRegencia = ObterTodos<AtividadeAvaliativaRegencia>();

            atividadeAvaliativasRegencia.ShouldNotBeEmpty();

            atividadeAvaliativasRegencia.Count().ShouldBeEquivalentTo(qtdeAtividadeRegencia);

            var atividadeAvaliativasDisciplina = ObterTodos<AtividadeAvaliativaDisciplina>();

            atividadeAvaliativasDisciplina.ShouldNotBeEmpty();

            atividadeAvaliativasDisciplina.Count().ShouldBeEquivalentTo(qtdeAtividadeDisciplina);
        }

        private async Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> ValidarEInserir(AtividadeAvaliativaDto atividadeAvaliativa, FiltroAtividadeAvaliativaDto filtroAtividadeAvaliativa)
        {
            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            async Task doExecutar() { await comando.Validar(filtroAtividadeAvaliativa); }

            await Should.NotThrowAsync(() => doExecutar());

            var retorno = await comando.Inserir(atividadeAvaliativa);

            return retorno;
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
                DataFim = DATA_29_04,
                TipoAvaliacao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                Bimestre = BIMESTRE_1
            };
        }
    }
}
