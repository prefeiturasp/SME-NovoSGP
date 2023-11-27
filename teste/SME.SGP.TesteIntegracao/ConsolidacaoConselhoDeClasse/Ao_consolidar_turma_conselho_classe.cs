using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nest;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse
{
    public class Ao_consolidar_turma_conselho_classe : TesteBaseComuns
    {
        public Ao_consolidar_turma_conselho_classe(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>), typeof(ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeConslidacaoConselho), ServiceLifetime.Scoped));

        }

        [Fact]
        public async Task Nao_deve_excluir_consolidacao_conselho_classe_turma_quando_aluno_estiver_com_matricula_dentro_do_periodo()
        {
            var turmaId = 1;
            var tipoCalendarioId = 1;

            await CriarDreUePerfilComponenteCurricular();
            await CriarTurma(Modalidade.Fundamental);
            await InserirNaBase(new TipoCalendario() { Id = tipoCalendarioId, Modalidade = ModalidadeTipoCalendario.FundamentalMedio, Nome = "1", Situacao = true, AnoLetivo = 2023, CriadoPor = "sistema", CriadoRF = "sistema", CriadoEm = DateTimeExtension.HorarioBrasilia() });
            await CriarPeriodosEscolares(tipoCalendarioId);
            await CriarConsolidacaoTurmaAluno(turmaId);
            await CriarConsolidacaoTurmaAlunoNota();

            var mensagemPorTurma = new ConsolidacaoTurmaDto(turmaId, (int)Bimestre.Primeiro);
            var jsonMensagem = JsonConvert.SerializeObject(mensagemPorTurma);


            var conselhoClasseConsolidadoTurmaAlunoNotaAntes = ObterTodos<ConselhoClasseConsolidadoTurmaAlunoNota>();

            var mediator = ServiceProvider.GetService<IMediator>();
            var executaConsolidacao = new ExecutarConsolidacaoTurmaConselhoClasseUseCase(mediator);
            await executaConsolidacao.Executar(new MensagemRabbit(jsonMensagem));

            var conselhoClasseConsolidadoTurmaAluno = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            var conselhoClasseConsolidadoTurmaAlunoNota = ObterTodos<ConselhoClasseConsolidadoTurmaAlunoNota>();
            Assert.NotNull(conselhoClasseConsolidadoTurmaAluno);
            Assert.NotEqual(conselhoClasseConsolidadoTurmaAlunoNota, conselhoClasseConsolidadoTurmaAlunoNotaAntes);

        }

        private async Task CriarConsolidacaoTurmaAlunoNota()
        {
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAlunoNota()
            {
                Bimestre = 1,
                ComponenteCurricularId = 139,
                ConceitoId = 1,
                ConselhoClasseConsolidadoTurmaAlunoId = 1,
                Id = 1,
                Nota = null
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAlunoNota()
            {
                Bimestre = 1,
                ComponenteCurricularId = 2,
                ConceitoId = 1,
                ConselhoClasseConsolidadoTurmaAlunoId = 1,
                Id = 2,
                Nota = null
            });

            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAlunoNota()
            {
                Bimestre = 1,
                ComponenteCurricularId = 139,
                ConceitoId = 1,
                ConselhoClasseConsolidadoTurmaAlunoId = 2,
                Id = 3,
                Nota = null
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAlunoNota()
            {
                Bimestre = 1,
                ComponenteCurricularId = 2,
                ConceitoId = 1,
                ConselhoClasseConsolidadoTurmaAlunoId = 2,
                Id = 4,
                Nota = null
            });


        }

        private async Task CriarConsolidacaoTurmaAluno(int turmaId)
        {
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 1,
                Status = 0,
                AlunoCodigo = ALUNO_CODIGO_1,
                TurmaId = turmaId,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno()
            {
                Id = 2,
                Status = 0,
                AlunoCodigo = ALUNO_CODIGO_2,
                TurmaId = turmaId,
                ParecerConclusivoId = null,
                CriadoRF = "1",
                CriadoPor = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
        }

        private async Task CriarPeriodosEscolares(int tipoCalendarioId)
        {
            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                Bimestre = (int)Bimestre.Primeiro,
                PeriodoInicio = new DateTime(2023, 02, 06),
                PeriodoFim = new DateTime(2023, 04, 29),
                TipoCalendarioId = tipoCalendarioId,
                CriadoPor = "sistema",
                CriadoRF = "sistema",
                CriadoEm = DateTimeExtension.HorarioBrasilia()

            });
            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 2,
                Bimestre = (int)Bimestre.Segundo,
                PeriodoInicio = new DateTime(2023, 05, 02),
                PeriodoFim = new DateTime(2023, 07, 08),
                TipoCalendarioId = tipoCalendarioId,
                CriadoPor = "sistema",
                CriadoRF = "sistema",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 3,
                Bimestre = (int)Bimestre.Terceiro,
                PeriodoInicio = new DateTime(2023, 07, 24),
                PeriodoFim = new DateTime(2023, 09, 30),
                TipoCalendarioId = tipoCalendarioId,
                CriadoPor = "sistema",
                CriadoRF = "sistema",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 4,
                Bimestre = (int)Bimestre.Quarto,
                PeriodoInicio = new DateTime(2023, 10, 02),
                PeriodoFim = new DateTime(2023, 12, 21),
                TipoCalendarioId = tipoCalendarioId,
                CriadoPor = "sistema",
                CriadoRF = "sistema",
                CriadoEm = DateTimeExtension.HorarioBrasilia()
            });


        }
    }
}
