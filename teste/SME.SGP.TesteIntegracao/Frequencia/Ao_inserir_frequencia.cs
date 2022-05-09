using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_inserir_frequencia : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;
        public Ao_inserir_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        //[Fact]
        public async Task Deve_inserir_frequencia_Compareceu()
        {
            await _builder.CriaItensComunsEja();

            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            var retorno = await useCase.Executar(ObtenhaFrenqueciaDto());

            retorno.ShouldNotBeNull();

            Assert.IsType<AuditoriaDto>(retorno);
        }

        private FrequenciaDto ObtenhaFrenqueciaDto()
        {
            var frenquencia = new FrequenciaDto(1);

            frenquencia.ListaFrequencia = ObtenhaListaDeFrequenciaAluno();

            return frenquencia;
        }

        private List<RegistroFrequenciaAlunoDto> ObtenhaListaDeFrequenciaAluno()
        {
            var lista = new List<RegistroFrequenciaAlunoDto>();
            var aulas = ObtenhaFrenquenciaAula();

            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "1", Aulas = aulas, TipoFrequenciaPreDefinido = "C" });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "2", Aulas = aulas, TipoFrequenciaPreDefinido = "C" });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "3", Aulas = aulas, TipoFrequenciaPreDefinido = "C" });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "4", Aulas = aulas, TipoFrequenciaPreDefinido = "C" });

            return lista;
        }

        private List<FrequenciaAulaDto> ObtenhaFrenquenciaAula()
        {
            var lista = new List<FrequenciaAulaDto>();

            lista.Add(new FrequenciaAulaDto() { NumeroAula = 1, TipoFrequencia = "C" });

            return lista;
        }
    }
}
