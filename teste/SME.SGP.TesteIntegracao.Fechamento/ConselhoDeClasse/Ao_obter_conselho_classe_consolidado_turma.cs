using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_obter_conselho_classe_consolidado_turma : TesteBaseComuns
    {
        public Ao_obter_conselho_classe_consolidado_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaConselhoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPareceresConclusivosQuery, IEnumerable<ConselhoClasseParecerConclusivoDto>>), typeof(ObterPareceresConclusivosQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterConsultaFrequenciaGeralAlunoQuery, string>), typeof(ObterConsultaFrequenciaGeralAlunoQueryHandlerFake), ServiceLifetime.Scoped));

        }

        [Fact]
        public async Task Deve_obter_a_quantidade_alunos_agrupado_situacao_conselho_na_turma_bimestre()
        {
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.Medio);
            await CriarConselhoClasseConsolidadoTurmaAlunos();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_3,DATA_FIM_BIMESTRE_3,BIMESTRE_3,TIPO_CALENDARIO_1);

            var filtro = new FiltroConselhoClasseConsolidadoTurmaBimestreDto(TURMA_ID_1, BIMESTRE_3, -99);

            var mediator = ServiceProvider.GetService<IMediator>();

            var obterConselhoClasseConsolidado = new ObterConselhoClasseConsolidadoPorTurmaBimestreUseCase(mediator);
            var resultado = await obterConselhoClasseConsolidado.Executar(filtro);
           
            var conselhoClasseConsolidadoTurmaAluno = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            var conselhoClasseConsolidadoTurmaAlunoAgrupado = conselhoClasseConsolidadoTurmaAluno.GroupBy(x => x.Status);
            var qtdeNaoIniciado = conselhoClasseConsolidadoTurmaAlunoAgrupado.First(x => x.Key == SituacaoConselhoClasse.NaoIniciado);
            var qtdeEmAndamento = conselhoClasseConsolidadoTurmaAlunoAgrupado.First(x => x.Key == SituacaoConselhoClasse.EmAndamento);
            var qtdeConcluido = conselhoClasseConsolidadoTurmaAlunoAgrupado.First(x => x.Key == SituacaoConselhoClasse.Concluido);

            Assert.Equal(qtdeNaoIniciado.Count(), resultado.First(x => x.Status == 0).Quantidade);
            Assert.Equal(qtdeEmAndamento.Count(), resultado.First(x => x.Status == 1).Quantidade);
            Assert.Equal(qtdeConcluido.Count(), resultado.First(x => x.Status == 2).Quantidade);
        }

     
    }
}