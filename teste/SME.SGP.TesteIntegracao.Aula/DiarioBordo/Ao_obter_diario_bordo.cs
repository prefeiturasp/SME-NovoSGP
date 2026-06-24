using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DiarioBordo
{
    [Collection("TesteIntegradoSGP")]
    public class Ao_obter_diario_bordo : DiarioBordoTesteBase
    {
        public Ao_obter_diario_bordo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Diário de Bordo - Deve obter diário com planejamento do componente irmão")]
        public async Task Ao_obter_diario_bordo_com_planejamento_irmao()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                ContemObservacoes = false,
                ContemDevolutiva = false
            };

            await CriarDadosBasicos(filtroDiarioBordo);

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_2,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_513,
                TurmaId = TURMA_ID_1,
                DevolutivaId = null,
                Planejamento = "Planejado",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            var useCase = ServiceProvider.GetService<IObterDiarioBordoUseCase>();
            var dto = await useCase.Executar(AULA_ID_1, COMPONENTE_CURRICULAR_512);

            dto.ShouldNotBeNull();
            dto.NomeComponenteIrmao.ShouldBe(COMPONENTE_REGENCIA_INFANTIL_EMEI_4H_NOME.Replace("'", ""));
            dto.PlanejamentoIrmao.ShouldBe("Planejado");
        }

        [Fact(DisplayName = "Diário de Bordo - Deve obter novo diário com planejamento do componente irmão existente")]
        public async Task Ao_obter_novo_diario_bordo_com_planejamento_irmao()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_513,
                ContemObservacoes = false,
                ContemDevolutiva = false
            };

            await CriarDadosBasicos(filtroDiarioBordo);

            var useCase = ServiceProvider.GetService<IObterDiarioBordoUseCase>();

            var dto = await useCase.Executar(AULA_ID_1, COMPONENTE_CURRICULAR_512);

            dto.ShouldNotBeNull();

            dto.NomeComponenteIrmao.ShouldBe(
                COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME.Replace("'", ""));

            dto.Planejamento.ShouldBeNullOrEmpty();

            dto.PlanejamentoIrmao.ShouldBe("Planejado");
        }
    }
}