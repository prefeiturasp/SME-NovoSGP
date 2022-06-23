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
    public class Ao_registrar_avaliacao_para_professor_regente : TesteAvaliacao
    {
        private readonly DateTime DATA_24_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 24);

        private const long TIPO_CALENDARIO_1 = 1;

        public Ao_registrar_avaliacao_para_professor_regente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Registrar_avaliacao_para_professor_regente_fundamental()
        {
            await CriarDadosBasicos(ObterCriacaoDeDadosDto());

            var comando = ServiceProvider.GetService<IComandosAtividadeAvaliativa>();

            await CriarPeriodoEscolarReabertura(TIPO_CALENDARIO_1);

            await CriarAula(DATA_24_01, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), TIPO_CALENDARIO_1);

            string[] disciplinaRegencia = { COMPONENTE_CIENCIAS_ID_89, COMPONENTE_GEOGRAFIA_ID_8, COMPONENTE_HISTORIA_ID_7, COMPONENTE_LINGUA_PORTUGUESA_ID_138 };

            var atividadeAvaliativa = ObterAtividadeAvaliativaRegenciaDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01, TipoAvaliacaoCodigo.AvaliacaoBimestral, disciplinaRegencia);

            var filtroAtividadeAvaliativa = ObterFiltroAtividadeAvaliativa(atividadeAvaliativa);

            async Task doExecutar() { await comando.Validar(filtroAtividadeAvaliativa); }

            await Should.NotThrowAsync(() => doExecutar());

            var retorno = await comando.Inserir(atividadeAvaliativa);

            retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();

            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);

            var atividadeAvaliativasRegencia = ObterTodos<AtividadeAvaliativaRegencia>();

            atividadeAvaliativasRegencia.ShouldNotBeEmpty();

            atividadeAvaliativasRegencia.Count().ShouldBeEquivalentTo(4);

            var atividadeAvaliativasDisciplina = ObterTodos<AtividadeAvaliativaDisciplina>();

            atividadeAvaliativasDisciplina.ShouldNotBeEmpty();

            atividadeAvaliativasDisciplina.Count().ShouldBeEquivalentTo(1);

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
