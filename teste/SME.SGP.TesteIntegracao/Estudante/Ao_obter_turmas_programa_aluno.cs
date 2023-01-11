using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Estudante.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Linq;

namespace SME.SGP.TesteIntegracao.TestarEstudante
{
    public class Ao_obter_turmas_programa_aluno : TesteBase
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

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmasAlunoPorFiltroQueryHandler_TurmasProgramaEstudanteFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmaCodigoQuery, IEnumerable<DisciplinaResposta>>), typeof(ObterComponentesCurricularesPorTurmaCodigoQueryHandler_TurmasProgramaEstudanteFake), ServiceLifetime.Scoped));
        }

        public Ao_obter_turmas_programa_aluno(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            collectionFixture.Services.Replace(
                new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorCodigoEolQueryHandlerFake), ServiceLifetime.Scoped));
            
            collectionFixture.BuildServiceProvider();
        }

        [Fact]
        public async Task Deve_obter_turmas_programa_estudante_por_codigo_e_anoletivo()
        {
            _builder = new ItensBasicosBuilder(this);
            
            await _builder.CriaItensComuns();

            var useCase = ServiceProvider.GetService<IObterEstudanteTurmasProgramaUseCase>();

            var retorno = await useCase.Executar(CODIGO_ALUNO, DateTimeExtension.HorarioBrasilia().Year, false);

            retorno.ShouldNotBeNull();
            retorno.FirstOrDefault().Turma.ShouldBe("EF - Turma Programa - Intermediário");
            retorno.FirstOrDefault().DreUe.ShouldBe("DRE AB Nome da UE");
            retorno.FirstOrDefault().ComponenteCurricular.ShouldBe("RECUPERAÇÃO DE APRENDIZAGENS");
        }


        private string ObterNomeTurma(string nome, string turno)
        {
            return $"{Modalidade.EJA.ShortName()} - {nome} - {turno}";
        }
    }
}
