using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Estudante.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarEstudante
{
    public class Ao_obter_turmas_programa_aluno : TesteBase
    {
        private ItensBasicosBuilder _builder;
        private const string CODIGO_ALUNO = "11223344";

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmasAlunoPorFiltroQueryHandler_TurmasProgramaEstudanteFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>), typeof(ObterComponentesCurricularesPorTurmaCodigoQueryHandler_TurmasProgramaEstudanteFake), ServiceLifetime.Scoped));
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
    }
}
