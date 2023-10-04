using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.HistoricoEscolar.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_obter_sintese : ConselhoDeClasseTesteBase
    {
        private const string BASE_NACIONAL_COMUM = "Base Nacional Comum"; 
            
        public Ao_obter_sintese(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>), typeof(ObterDisciplinasPorCodigoTurmaQueryHandlerComponente512Fake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Conselho Classe - Deve retornar a síntese do aluno de componente que não lança nota - Infantil")]
        public async Task Ao_obter_sintese_aluno_deve_retornar_componente_infantil()
        {
            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                Bimestre = BIMESTRE_4,
                ComponenteCurricular = COMPONENTE_CURRICULAR_512.ToString(),
                DataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                CriarPeriodoReabertura = true,
            };
            
            await CriarDadosBase(filtroNota);

            var obterSinteseConselhoDeClasseUseCase = ServiceProvider.GetService<IObterSinteseConselhoDeClasseUseCase>();

            var conselhoClasseFechamentoAluno = new ConselhoClasseSinteseDto(0, FECHAMENTO_TURMA_ID_1, ALUNO_CODIGO_1, TURMA_CODIGO_1, BIMESTRE_4);
            
            var retorno = await obterSinteseConselhoDeClasseUseCase.Executar(conselhoClasseFechamentoAluno);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(1);
            retorno.FirstOrDefault().Id.ShouldBe(1);
            retorno.FirstOrDefault().Titulo.ShouldBe(BASE_NACIONAL_COMUM);
            retorno.FirstOrDefault().ComponenteSinteses.Count().ShouldBe(1);
            retorno.FirstOrDefault().ComponenteSinteses.Any(a=> a.Codigo == COMPONENTE_CURRICULAR_512).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Conselho Classe - Não deve retornar a síntese quando não possui fechamento da turma")]
        public async Task Ao_obter_sintese_aluno_nao_deve_retornar_sem_fechamento_turma()
        {
            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                Bimestre = BIMESTRE_4,
                ComponenteCurricular = COMPONENTE_CURRICULAR_512.ToString(),
                DataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                CriarPeriodoReabertura = true,
            };
            
            await CriarDadosBase(filtroNota);

            var obterSinteseConselhoDeClasseUseCase = ServiceProvider.GetService<IObterSinteseConselhoDeClasseUseCase>();

            var conselhoClasseFechamentoAluno = new ConselhoClasseSinteseDto(0, 0, ALUNO_CODIGO_1, TURMA_CODIGO_1, BIMESTRE_4);
            
            await Assert.ThrowsAsync<NegocioException>(() => obterSinteseConselhoDeClasseUseCase.Executar(conselhoClasseFechamentoAluno));
        }
    }
}