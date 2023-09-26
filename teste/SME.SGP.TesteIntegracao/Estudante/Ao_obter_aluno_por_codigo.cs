using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarEstudante
{
    public class Ao_obter_aluno_por_codigo : TesteBase
    {
        private ItensBasicosBuilder _builder;
        private const string CODIGO_ALUNO = "11223344";
        private const int ANO_2022 = 2022;
        private const string CODIGO_TURMA_1 = "1";
        private const string CODIGO_TURMA_2 = "2";
        private const string NOME_TURMA_1 = "Turma Nome 1";
        private const string NOME_TURMA_2 = "Turma Nome 2";
        private const int ID_UE = 1;
        private const string TIPO_TURNO_TARDE = "Tarde";
        private const string TIPO_TURNO_INTERMEDIARIO = "Intermediário";

        public Ao_obter_aluno_por_codigo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            collectionFixture.Services.Replace(
                new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorCodigoEolQueryHandlerFake), ServiceLifetime.Scoped));
            
            collectionFixture.BuildServiceProvider();
        }

        [Fact]
        public async Task Deve_obter_estudante_por_codigo_e_turma()
        {
            _builder = new ItensBasicosBuilder(this);
            
            await _builder.CriaItensComunsEja();

            var useCase = ServiceProvider.GetService<IObterAlunoPorCodigoEolEAnoLetivoUseCase>();

            var retorno = await useCase.Executar(CODIGO_ALUNO, ANO_2022, CODIGO_TURMA_1);

            retorno.ShouldNotBeNull();

            retorno.TurmaEscola.ShouldBe(ObterNomeTurma(NOME_TURMA_1, TIPO_TURNO_INTERMEDIARIO));
        }

        [Fact]
        public async Task Deve_obter_estudante_por_codigo()
        {
            _builder = new ItensBasicosBuilder(this);
            
            await _builder.CriaItensComunsEja();

            var useCase = ServiceProvider.GetService<IObterAlunoPorCodigoEolEAnoLetivoUseCase>();

            var retorno = await useCase.Executar(CODIGO_ALUNO, ANO_2022, null);

            retorno.ShouldNotBeNull();

            retorno.TurmaEscola.ShouldBe(ObterNomeTurma(NOME_TURMA_1, TIPO_TURNO_INTERMEDIARIO));
        }

        private async Task CriaTurma2()
        {
            await InserirNaBase(new Turma
            {
                UeId = ID_UE,
                Ano = "2",
                CodigoTurma = CODIGO_TURMA_2,
                Historica = true,
                ModalidadeCodigo = Modalidade.EJA,
                AnoLetivo = ANO_2022,
                Semestre = 2,
                Nome = NOME_TURMA_2,
                TipoTurno = 2
            });
        }

        private string ObterNomeTurma(string nome, string turno)
        {
            return $"{Modalidade.EJA.ShortName()} - {nome} - {turno}";
        }
    }
}
