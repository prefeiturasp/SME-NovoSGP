using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.Base;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_gerar_parecer_conclusivo : ConselhoClasseTesteBase
    {
        public Ao_gerar_parecer_conclusivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>),typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake),ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>),typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake),ServiceLifetime.Scoped));
            
        }

        [Fact]
        public async Task Ao_gerar_parecer_conclusivo_aluno()
        {
            var filtroConselhoClasse = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                InserirConselhoClassePadrao = true,
                InserirFechamentoAlunoPadrao = true,
                CriarPeriodoEscolar = true,
                TipoNota = TipoNota.Nota,
                AnoTurma = "6"
            };

            await CriarDadosBase(filtroConselhoClasse);

            var conselhoClasseFechamentoAluno = new ConselhoClasseFechamentoAlunoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = 1,
                FechamentoTurmaId = 1
            };
            
            var gerarParecerConclusivoUseCase = RetornarGerarParecerConclusivoUseCase();

            var retorno = await gerarParecerConclusivoUseCase.Executar(conselhoClasseFechamentoAluno);

            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Nome.ShouldNotBeEmpty();
        }
        
        [Fact]
        public async Task Ao_reprocessar_parecer_conclusivo_aluno()
        {
            var filtroConselhoClasse = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                InserirConselhoClassePadrao = true,
                InserirFechamentoAlunoPadrao = true,
                CriarPeriodoEscolar = true,
                TipoNota = TipoNota.Nota,
                AnoTurma = "6"
            };

            await CriarDadosBase(filtroConselhoClasse);

            var conselhoClasseFechamentoAluno = new ConselhoClasseFechamentoAlunoDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                ConselhoClasseId = 1,
                FechamentoTurmaId = 1
            };

            var reprocessarParecerConclusivoAlunoUseCase = RetornarReprocessarParecerConclusivoAlunoUseCase();

            var retorno = await reprocessarParecerConclusivoAlunoUseCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(conselhoClasseFechamentoAluno)));

            retorno.ShouldBeTrue();

            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.Any(f=> f.ConselhoClasseParecerId > 0).ShouldBeTrue();
        }
    }
}