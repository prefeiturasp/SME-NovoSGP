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

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_consolidar_frequencia_turma_mensal : TesteBaseComuns
    {
        public Ao_consolidar_frequencia_turma_mensal(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Ao consolidar frequência de turma mensal do mês")]
        public async Task Ao_consolidar_frequencia_turma_mensal_do_mes()
        {
            var mesAnterior = DateTimeExtension.HorarioBrasilia().Month - 1;
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Medio, ANO_5, TURMA_CODIGO_1);
            await CriaAulaFrequencia(AULA_ID_1, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 01), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_2, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 05), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_3, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 10), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_4, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 15), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_5, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 16), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_6, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 20), TipoFrequencia.F, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_7, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 25), TipoFrequencia.F, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_8, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 28), TipoFrequencia.R, TipoFrequencia.C);

            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaPorTurmaMensalUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaTurma(TURMA_ID_1, TURMA_CODIGO_1, 50, new(DateTimeExtension.HorarioBrasilia().Year, DateTimeExtension.HorarioBrasilia().Month, 01));
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacao = ObterTodos<ConsolidacaoFrequenciaTurma>();
            consolidacao.ShouldNotBeEmpty();
            consolidacao.Count.ShouldBe(1);
            var consolidado = consolidacao.FirstOrDefault();
            consolidado.ShouldNotBeNull();
            consolidado.TipoConsolidado.ShouldBe(TipoConsolidadoFrequencia.Mensal);
            consolidado.QuantidadeAcimaMinimoFrequencia.ShouldBe(1);
            consolidado.QuantidadeAbaixoMinimoFrequencia.ShouldBe(1);
        }

        [Fact(DisplayName = "Ao consolidar frequência de turma mensal com mais meses")]
        public async Task Ao_consolidar_frequencia_turma_mensal_com_mais_meses()
        {
            var mesAnterior = DateTimeExtension.HorarioBrasilia().Month - 1;
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Medio, ANO_5, TURMA_CODIGO_1);
            await CriaAulaFrequencia(AULA_ID_1, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 01), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_2, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 05), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_3, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 10), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_4, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 15), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_5, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 16), TipoFrequencia.C, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_6, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 20), TipoFrequencia.F, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_7, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 25), TipoFrequencia.F, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_8, new(DateTimeExtension.HorarioBrasilia().Year, mesAnterior, 28), TipoFrequencia.R, TipoFrequencia.C);

            await CriaAulaFrequencia(AULA_ID_9, new(DateTimeExtension.HorarioBrasilia().Year, DateTimeExtension.HorarioBrasilia().Month, 01), TipoFrequencia.C, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_10, new(DateTimeExtension.HorarioBrasilia().Year, DateTimeExtension.HorarioBrasilia().Month, 05), TipoFrequencia.C, TipoFrequencia.C);

            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaPorTurmaMensalUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaTurma(TURMA_ID_1, TURMA_CODIGO_1, 50, new(DateTimeExtension.HorarioBrasilia().Year, DateTimeExtension.HorarioBrasilia().Month, 01));
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacao = ObterTodos<ConsolidacaoFrequenciaTurma>();
            consolidacao.ShouldNotBeEmpty();
            consolidacao.Count.ShouldBe(1);
            var consolidado = consolidacao.FirstOrDefault();
            consolidado.ShouldNotBeNull();
            consolidado.TipoConsolidado.ShouldBe(TipoConsolidadoFrequencia.Mensal);
            consolidado.QuantidadeAcimaMinimoFrequencia.ShouldBe(2);
            consolidado.QuantidadeAbaixoMinimoFrequencia.ShouldBe(0);
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
                CodigoAluno = CODIGO_ALUNO_1,
                RegistroFrequenciaId = aulaId,
                CriadoPor = "",
                CriadoRF = "",
                Valor = (int)tipoAluno1,
                NumeroAula = 1,
                AulaId = aulaId
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_2,
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
