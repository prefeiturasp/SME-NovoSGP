using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DiarioBordo
{
    public class Ao_obter_diario_bordo : DiarioBordoTesteBase
    {
        public Ao_obter_diario_bordo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_diario_bordo_com_planejamento_irmao()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                ContemObservacoes = false,
                ContemDevolutiva = false
            };

            await CriarDadosBasicos(filtroDiarioBordo);
            filtroDiarioBordo.ComponenteCurricularId = COMPONENTE_CURRICULAR_513;
            await CriarDiarioBordo(filtroDiarioBordo);

            var useCase = ServiceProvider.GetService<IObterDiarioBordoUseCase>();
            var dto = await useCase.Executar(AULA_ID_1, COMPONENTE_CURRICULAR_512);

            dto.ShouldNotBeNull();
            dto.NomeComponenteIrmao.ShouldBe(COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME.Replace("'", ""));
            dto.PlanejamentoIrmao.ShouldBe("Planejado");
        }

        [Fact]
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
            dto.NomeComponenteIrmao.ShouldBe(COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME.Replace("'", ""));
            dto.Planejamento.ShouldBeNullOrEmpty();
            dto.PlanejamentoIrmao.ShouldBe("Planejado");
        }
    }
}
