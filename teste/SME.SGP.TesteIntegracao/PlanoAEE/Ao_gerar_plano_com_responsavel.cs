using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_gerar_plano_com_responsavel : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public Ao_gerar_plano_com_responsavel(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact(DisplayName = "Plano AEE - Deve gerar um plano aee com responsavel")]
        public async Task Deve_gerar_plano_com_responsavel()
        {
            await _builder.CriaItensComuns();

            var useCase = ServiceProvider.GetService<ISalvarPlanoAEEUseCase>();

            var dto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = "11223344",
                ResponsavelRF = "2222222",
                TurmaId = 1,
                TurmaCodigo = "1",
                Questoes = new List<PlanoAEEQuestaoDto>(),
                Situacao = SituacaoPlanoAEE.ParecerCP
            };
            
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            retorno.PlanoId.ShouldBe(1);
        }
    }
}
