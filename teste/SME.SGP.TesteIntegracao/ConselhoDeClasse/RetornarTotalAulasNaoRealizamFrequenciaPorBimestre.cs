using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AlunosSemFrequencia
{
    public class RetornarTotalAulasNaoRealizamFrequenciaPorBimestre : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public RetornarTotalAulasNaoRealizamFrequenciaPorBimestre(CollectionFixture testFixture) : base(testFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact]
        public async Task Deve_Obter_Total_Aulas_Nao_Realizam_Frequencia_Por_Bimestre()
        {
            await _builder.CriaItensComunsEja();
            await _builder.CriaAulaSemFrequencia();
            await _builder.CriaComponenteCurricularSemFrequencia();

            var useCase = ServiceProvider.GetService<IObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase>();

            var retorno = await useCase.Executar("1106", "1", 1);
           
            retorno.ShouldNotBeNull();

            Assert.True(retorno.Any());
        }
    }
}
