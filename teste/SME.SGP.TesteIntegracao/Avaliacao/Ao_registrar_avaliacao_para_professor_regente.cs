using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAvaliacaoAula
{
    public class Ao_registrar_avaliacao_para_professor_regente : TesteAvaliacao
    {
        private readonly DateTime DATA_03_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 03);
        private readonly DateTime DATA_29_04 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 29);

        private readonly DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private readonly DateTime DATA_08_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        private readonly DateTime DATA_25_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        private readonly DateTime DATA_30_09 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);

        private readonly DateTime DATA_03_10 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        private readonly DateTime DATA_22_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);

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
            //var atividadeAvaliativa = ObterAtividadeAvaliativaDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1213.ToString(), CategoriaAtividadeAvaliativa.Normal, DATA_24_01);

            await CriarPeriodoEscolarReabertura();

            //var retorno = await comando.Inserir(atividadeAvaliativa);

            //retorno.ShouldNotBeNull();

            var atividadeAvaliativas = ObterTodos<AtividadeAvaliativa>();

            atividadeAvaliativas.ShouldNotBeEmpty();
            atividadeAvaliativas.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        private async Task CriarPeriodoEscolarReabertura()
        {
            await CriarPeriodoEscolar(DATA_03_01, DATA_29_04, BIMESTRE_1, TIPO_CALENDARIO_1);
            await CriarPeriodoEscolar(DATA_02_05, DATA_08_07, BIMESTRE_2, TIPO_CALENDARIO_1);
            await CriarPeriodoEscolar(DATA_25_07, DATA_30_09, BIMESTRE_3, TIPO_CALENDARIO_1);
            await CriarPeriodoEscolar(DATA_03_10, DATA_22_12, BIMESTRE_4, TIPO_CALENDARIO_1);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        private CriacaoDeDadosDto ObterCriacaoDeDadosDto()
        {
            return new CriacaoDeDadosDto()
            {
                Perfil = ObterPerfilProfessor(),
                ModalidadeTurma = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                DataInicio = DATA_02_05,
                DataFim = DATA_08_07,
                TipoAvaliacao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                Bimestre = BIMESTRE_2
            };
        }
    }
}
