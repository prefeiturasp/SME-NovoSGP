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

namespace SME.SGP.TesteIntegracao.FechamentoAcompanhamentoConselhoDeClasse
{
    public class Ao_obter_detalhamento_da_turma : TesteBaseComuns
    {
        public Ao_obter_detalhamento_da_turma(CollectionFixture collectionFixture) : base(collectionFixture)
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
        public async Task Deve_obter_os_alunos_e_situacoes_do_conselho_de_classe()
        {
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.Medio);
            await CriarConselhoClasseConsolidadoTurmaAlunos();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_3, DATA_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1);

            var filtro = new FiltroConselhoClasseConsolidadoTurmaBimestreDto(TURMA_ID_1, BIMESTRE_3, -99);
            var mediator = ServiceProvider.GetService<IMediator>();


            var conselhoClasseConsolidadoTurmaAluno = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();

            var obterConselhoClasseConsolidado = new ObterFechamentoConselhoClasseAlunosPorTurmaUseCase(mediator);
            var resultado = await obterConselhoClasseConsolidado.Executar(filtro);

            var teste = resultado.Any(x => conselhoClasseConsolidadoTurmaAluno.Any(b => b.AlunoCodigo == x.AlunoCodigo && x.SituacaoFechamentoCodigo == (int)b.Status));
            Assert.True(teste);           

        }
    }
}
