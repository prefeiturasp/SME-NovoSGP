using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_inserir_nota_pos_conselho_aluno_inativo_durante_periodo_reabertura : ConselhoDeClasseTesteBase
    {
        public Ao_inserir_nota_pos_conselho_aluno_inativo_durante_periodo_reabertura(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosInativoDurantePeriodoReaberturaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_lancar_nota_durante_periodo_de_abertura()
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, FECHAMENTO_TURMA_ID_4, BIMESTRE_4);

            var obterFiltroConselhoClasse = ObterFiltroConselhoClasse(ObterPerfilProfessor(),
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                TipoNota.Conceito,
                ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false);

            await CriarDadosBaseSemFechamentoTurmaSemAberturaReabertura(obterFiltroConselhoClasse);
            await CriarPeriodoEscolarCustomizadoQuartoBimestre();
            await CriarFechamentoTurmaDisciplinaAlunoNota(obterFiltroConselhoClasse);
            await CriarPeriodoReabertura(obterFiltroConselhoClasse.TipoCalendarioId);

            await ExecutarComandoSalvarConselho(salvarConselhoClasseAlunoNotaDto);
        }
        //bulk insert
        //[Fact]
        public async Task Deve_inserir_anotacoes_para_inativo_durante_periodo()
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito);
            await CriarDados(ObterPerfilProfessor(), salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular, TipoNota.Conceito, ANO_4, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false, SituacaoConselhoClasse.EmAndamento, false);
            await InserirConselhoDeClasse();
            var dto = ObterAnotacoesDto();

            var retorno = await ExecutarComandoSalvarAnotacoesConselhoClasseAluno(dto);

            retorno.AnotacoesPedagogicas.ShouldNotBeNullOrEmpty();
            retorno.RecomendacoesFamilia.ShouldNotBeNullOrEmpty();
            retorno.RecomendacoesAluno.ShouldNotBeNullOrEmpty();
        }

        private async Task ExecutarComandoSalvarConselho(SalvarConselhoClasseAlunoNotaDto conselhoClasse)
        {
            var comando = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();

            var dtoRetorno = await comando.Executar(conselhoClasse);

            dtoRetorno.ShouldNotBeNull();
        }

        private async Task<ConselhoClasseAluno> ExecutarComandoSalvarAnotacoesConselhoClasseAluno(ConselhoClasseAlunoAnotacoesDto dto)
        {
            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();
            return await useCase.Executar(dto);
        }

        private FiltroConselhoClasseDto ObterFiltroConselhoClasse(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_03_10_INICIO_BIMESTRE_4.AddYears(-1) : DATA_03_10_INICIO_BIMESTRE_4;

            return new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_4,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse
            };
        }

        private ConselhoClasseAlunoAnotacoesDto ObterAnotacoesDto()
        {
            return new ConselhoClasseAlunoAnotacoesDto()
            {
                AlunoCodigo = ALUNO_CODIGO_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                AnotacoesPedagogicas = "Teste",
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                RecomendacaoAluno = "teste",
                RecomendacaoAlunoIds = new List<long>() { 1 },
                RecomendacaoFamilia = "teste",
                RecomendacaoFamiliaIds = new List<long>() { 1 }
            };
        }
        private async Task InserirConselhoDeClasse()
        {
            var fechamento = ObterTodos<FechamentoTurma>().FirstOrDefault();
            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                Situacao = SituacaoConselhoClasse.EmAndamento,
                FechamentoTurma = fechamento,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        private async Task CriarDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse
            };

            await CriarDadosBase(filtroNota);
        }
    }
}