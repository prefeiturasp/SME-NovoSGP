using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConsolidacaoFrequenciaMensal.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsolidacaoFrequenciaMensal
{
    public class Ao_consolidar_frequencia_turma_semanal : TesteBaseComuns
    {
        public Ao_consolidar_frequencia_turma_semanal(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Ao consolidar frequência de turma semanal completa")]
        public async Task Ao_consolidar_frequencia_turma_semanal_completa()
        { 
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Medio, ANO_5, TURMA_CODIGO_1);
            await CriaAulaFrequencia(AULA_ID_1, new(2023, 03, 06), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_2, new(2023, 03, 07), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_3, new(2023, 03, 08), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_4, new(2023, 03, 09), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_4, new(2023, 03, 10), TipoFrequencia.C, TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaPorTurmaSemanalUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaTurma(TURMA_ID_1, TURMA_CODIGO_1, 50, new(2023, 03, 11));
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacao = ObterTodos<ConsolidacaoFrequenciaTurma>();
            consolidacao.ShouldNotBeEmpty();
            consolidacao.Count.ShouldBe(1);
            var consolidado = consolidacao.FirstOrDefault();
            consolidado.ShouldNotBeNull();
            consolidado.TipoConsolidacao.ShouldBe(TipoConsolidadoFrequencia.Semanal);
            consolidado.QuantidadeAcimaMinimoFrequencia.ShouldBe(1);
            consolidado.QuantidadeAbaixoMinimoFrequencia.ShouldBe(1);
        }

        [Fact(DisplayName = "Ao consolidar frequência de turma semanal incompleta")]
        public async Task Ao_consolidar_frequencia_turma_semanal_incompleta()
        {
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Medio, ANO_5, TURMA_CODIGO_1);
            await CriaAulaFrequencia(AULA_ID_1, new(2023, 03, 06), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_2, new(2023, 03, 07), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_3, new(2023, 03, 08), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_4, new(2023, 03, 09), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_5, new(2023, 03, 10), TipoFrequencia.C, TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaPorTurmaSemanalUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaTurma(TURMA_ID_1, TURMA_CODIGO_1, 50, new(2023, 03, 14));
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacao = ObterTodos<ConsolidacaoFrequenciaTurma>();
            consolidacao.ShouldNotBeEmpty();
            consolidacao.Count.ShouldBe(1);
            var consolidado = consolidacao.FirstOrDefault();
            consolidado.ShouldNotBeNull();
            consolidado.TipoConsolidacao.ShouldBe(TipoConsolidadoFrequencia.Semanal);
            consolidado.QuantidadeAcimaMinimoFrequencia.ShouldBe(1);
            consolidado.QuantidadeAbaixoMinimoFrequencia.ShouldBe(1);
        }

        [Fact(DisplayName = "Ao consolidar frequência de turma semanal com mais semanas ")]
        public async Task Ao_consolidar_frequencia_turma_semanal_com_mais_semanas()
        {
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Medio, ANO_5, TURMA_CODIGO_1);
            await CriaAulaFrequencia(AULA_ID_1, new(2023, 03, 06), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_2, new(2023, 03, 07), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_3, new(2023, 03, 08), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_4, new(2023, 03, 09), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_5, new(2023, 03, 10), TipoFrequencia.C, TipoFrequencia.F);

            await CriaAulaFrequencia(AULA_ID_6, new(2023, 03, 13), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_7, new(2023, 03, 14), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_8, new(2023, 03, 15), TipoFrequencia.C, TipoFrequencia.F);

            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaPorTurmaSemanalUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaTurma(TURMA_ID_1, TURMA_CODIGO_1, 50, new(2023, 03, 14));
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacao = ObterTodos<ConsolidacaoFrequenciaTurma>();
            consolidacao.ShouldNotBeEmpty();
            consolidacao.Count.ShouldBe(1);
            var consolidado = consolidacao.FirstOrDefault();
            consolidado.ShouldNotBeNull();
            consolidado.TipoConsolidacao.ShouldBe(TipoConsolidadoFrequencia.Semanal);
            consolidado.QuantidadeAcimaMinimoFrequencia.ShouldBe(1);
            consolidado.QuantidadeAbaixoMinimoFrequencia.ShouldBe(1);
        }

        private async Task CriaAulaFrequencia(long aulaId, DateTime data, TipoFrequencia tipoAluno1, TipoFrequencia tipoAluno2)
        {
            await InserirNaBase(new Dominio.Aula
            {
                CriadoPor = "",
                CriadoRF = "",
                UeId = UE_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                ProfessorRf = "",
                TipoCalendarioId = TIPO_CALENDARIO_1,
                DataAula = data,
                Quantidade = 1
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = aulaId,
                AulaId = aulaId,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ALUNO_CODIGO_4,
                RegistroFrequenciaId = aulaId,
                CriadoPor = "",
                CriadoRF = "",
                Valor = (int)tipoAluno1,
                NumeroAula = 1,
                AulaId = aulaId
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ALUNO_CODIGO_5,
                RegistroFrequenciaId = aulaId,
                CriadoPor = "",
                CriadoRF = "",
                Valor = (int)tipoAluno2,
                NumeroAula = 1,
                AulaId = aulaId
            });
        }
    }
}
