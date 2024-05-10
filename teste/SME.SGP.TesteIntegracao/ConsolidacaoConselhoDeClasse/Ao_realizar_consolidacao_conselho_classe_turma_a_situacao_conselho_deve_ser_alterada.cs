using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using SME.SGP.Dominio;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse
{
    public class Ao_realizar_consolidacao_conselho_classe_turma_a_situacao_conselho_deve_ser_alterada : TesteBaseComuns
    {
        public Ao_realizar_consolidacao_conselho_classe_turma_a_situacao_conselho_deve_ser_alterada(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ConselhoDeClasse.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesComNotaDeFechamentoOuConselhoQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesComNotaDeFechamentoOuConselhoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>), typeof(ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommand, long>), typeof(SalvarConselhoClasseConsolidadoTurmaAlunoNotaCommandHandlerFake), ServiceLifetime.Scoped));

        }
        [Fact]
        public async Task Ao_consolidar_conselho_classe_turma_deve_alterar_situacao_conselho_classe_se_houver_nota()
        {
            var alunoCodigo = "1";
            var bimestre = 1;
            var turmaId = 1;
            var inativo = false;
            var componenteCurricularId = 0;

            await CriarDreUePerfilComponenteCurricular();
            await CriarTurma(Modalidade.Medio);
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 1,
                Status = 0,
                AlunoCodigo = alunoCodigo,
                TurmaId = turmaId,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            var mensagemRabbit = new MensagemConsolidacaoConselhoClasseAlunoDto()
            {
                AlunoCodigo = alunoCodigo,
                TurmaId = turmaId,
                Inativo = inativo,
                ComponenteCurricularId = componenteCurricularId,
                Bimestre = bimestre,
            };

            var jsonMensagem = JsonSerializer.Serialize(mensagemRabbit);
            var mediator = ServiceProvider.GetService<IMediator>();

            var executaConsolidacao = new ExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase(mediator);
            await executaConsolidacao.Executar(new MensagemRabbit(jsonMensagem));

            var conselhoClasseConsolidadoTurmaAluno = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            Assert.True(conselhoClasseConsolidadoTurmaAluno.First().Status != SituacaoConselhoClasse.NaoIniciado);
        }
    }
}
