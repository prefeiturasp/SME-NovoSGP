using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_obter_recomendacoes_notas : ConselhoDeClasseTesteBase
    {
        private const long TURMA_ID = 1;
        private const int PRIMEIRO_BIMESTRE = 1;

        public Ao_obter_recomendacoes_notas(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery,IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasComplementaresPorAlunoQuery, IEnumerable<TurmaComplementarDto>>), typeof(ObterTurmasComplementaresPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPerfilAtualQuery, Guid>), typeof(ObterPerfilAtualQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorTurmasCodigoQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificarSeExisteRecomendacaoPorTurmaQuery, IEnumerable<AlunoTemRecomandacaoDto>>), typeof(VerificarSeExisteRecomendacaoPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterConselhoClasseAlunoNotaQuery, IEnumerable<ConselhoClasseAlunoNotaDto>>), typeof(ObterConselhoClasseAlunoNotaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Obter Alunos Sem fechamento turma")]
        public async Task Obter_Alunos_Sem_Fechamento_Turma()
        {
            await CriarItensBasicos();
            var userCase = ServiceProvider.GetService<IObterAlunosSemNotasRecomendacoesUseCase>();

            var filtro = new FiltroInconsistenciasAlunoFamiliaDto(TURMA_ID, PRIMEIRO_BIMESTRE);
            await userCase.Executar(filtro).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Obter Alunos Sem recomendações e Notas")]
        public async Task Obter_Alunos_Sem_Notas_Recomendacoes()
        {
            await CriarItensBasicos();
            var userCase = ServiceProvider.GetService<IObterAlunosSemNotasRecomendacoesUseCase>();

            var filtro = new FiltroInconsistenciasAlunoFamiliaDto(TURMA_ID, PRIMEIRO_BIMESTRE);

            await InserirNaBase(new FechamentoTurma()
            {
                Id = 1,
                PeriodoEscolarId = 1,
                TurmaId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = 1,
                Situacao = SituacaoConselhoClasse.EmAndamento,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            var retornoUseCase = await userCase.Executar(filtro);
            retornoUseCase.Count().ShouldBeGreaterThan(0);
            retornoUseCase.FirstOrDefault().Inconsistencias.Count().ShouldBeGreaterThan(0);
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year
            });
            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 03),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 12, 29),
                Bimestre = 1,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 15, 23, 48, 43),
                CriadoPor = "Sistema",
                AlteradoEm = null,
                AlteradoPor = "",
                CriadoRF = "0",
                AlteradoRF = null,
                Migrado = false
            });
            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Calendário Teste Ano Atual",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                Periodo = Periodo.Anual,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
        }
    }
}