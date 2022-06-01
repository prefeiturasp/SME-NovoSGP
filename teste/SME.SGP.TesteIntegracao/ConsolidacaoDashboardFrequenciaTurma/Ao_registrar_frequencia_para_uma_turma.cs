using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_registrar_frequencia_para_uma_turma : TesteBase
    {

        public Ao_registrar_frequencia_para_uma_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_atualizar_somente_a_turma_em_especifico()
        {
            await CriarItensBasicos();

            await InserirNaBase(new Aula
            {
                Id = 1,
                CriadoPor = "",
                CriadoRF = "",
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                ProfessorRf = "",
                TipoCalendarioId = 1,
                DataAula = new DateTime(2022, 02, 02),
                Quantidade = 1
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = 1,
                AulaId = 1,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 1,
                CodigoAluno = "1",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 1,
                NumeroAula = 1
            });

            await InserirNaBase(new ConsolidacaoDashBoardFrequencia 
            {
                Id = 1,
                TurmaId = 1,
                TurmaAno = "7",
                DataAula = new DateTime(2022,2,2),
                ModalidadeCodigo = 5,
                Tipo = 3,
                AnoLetivo = 2022,
                DreId = 1,
                UeId = 1,
                DreCodigo = "1",
                DreAbreviacao = "CL",
                QuantidadeAusentes = 1,
                QuantidadePresencas = 20, 
                QuantidadeRemotos = 0,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new ConsolidacaoDashBoardFrequencia
            {
                Id = 1,
                TurmaId = 2,
                TurmaAno = "8",
                DataAula = new DateTime(2022, 2, 2),
                ModalidadeCodigo = 5,
                Tipo = 3,
                AnoLetivo = 2022,
                DreId = 1,
                UeId = 1,
                DreCodigo = "1",
                DreAbreviacao = "CL",
                QuantidadeAusentes = 5,
                QuantidadePresencas = 18,
                QuantidadeRemotos = 0,
                CriadoEm = DateTime.Now
            });

            var useCase = ServiceProvider.GetService<IExecutaConsolidacaoDashBoardFrequenciaPorUeUseCase>();
            var mensagem = new ConsolidacaoPorUeDashBoardFrequencia() { AnoLetivo = 2022, Mes = 2, TipoPeriodo = Dominio.Enumerados.TipoPeriodoDashboardFrequencia.Mensal, UeCodigo = "1"};
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 1,
                CodigoAluno = "1",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 1
            });

            var consolidacoes = ObterTodos<ConsolidacaoDashBoardFrequencia>();

            consolidacoes.ShouldNotBeEmpty();

            consolidacoes.Count.ShouldBe(1);
            consolidacoes.FirstOrDefault().Mes.ShouldBe(2);
            consolidacoes.FirstOrDefault(c => c.TurmaId == 1).QuantidadeAusentes.ShouldBe(5);
            consolidacoes.FirstOrDefault(c=> c.TurmaId == 1).QuantidadePresencas.ShouldBe(18);
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

            await InserirNaBase(new Turma
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                AnoLetivo = 2022
            });

            await InserirNaBase(new Turma
            {
                Id = 2,
                UeId = 1,
                Ano = "2",
                CodigoTurma = "2",
                AnoLetivo = 2022
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                Nome = "",
                CriadoPor = "",
                CriadoRF = ""
            });
        }
    }
}
