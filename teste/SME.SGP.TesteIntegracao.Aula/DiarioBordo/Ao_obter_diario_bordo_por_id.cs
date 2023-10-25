using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DiarioBordo
{
    public class Ao_obter_diario_bordo_por_id : DiarioBordoTesteBase
    {
        private const int DIARIO_BORDO_ID = 1;
        public Ao_obter_diario_bordo_por_id(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_diario_bordo_por_id_com_planejamento_irmao()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ContemObservacoes = true,
                ContemDevolutiva = false
            };

            await CriarDadosBasicos(filtroDiarioBordo);
            filtroDiarioBordo.ComponenteCurricularId = COMPONENTE_CURRICULAR_513;
            await CriarDiarioBordo(filtroDiarioBordo);

            var useCase = ServiceProvider.GetService<IObterDiarioBordoPorIdUseCase>();
            var dto = await useCase.Executar(DIARIO_BORDO_ID);

            dto.ShouldNotBeNull();
            dto.NomeComponenteIrmao.ShouldBe(COMPONENTE_REGENCIA_INFANTIL_EMEI_2H_NOME.Replace("'", ""));
            dto.Planejamento.ShouldBe("Planejado");
            dto.PlanejamentoIrmao.ShouldBe("Planejado");
        }

    }
}
