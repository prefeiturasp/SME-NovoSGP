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

namespace SME.SGP.TesteIntegracao.ConsolidacaoFrequenciaAlunoMensal
{
    public class Ao_registrar_consolidacao_frequencia_aluno_mensal : TesteBase
    {
        public Ao_registrar_consolidacao_frequencia_aluno_mensal(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_gravar_frequencia_consolidada_mes_4_e_100_de_percentual()
        {
            await CriarItensBasicos();

            await InserirNaBase(new Dominio.Aula
            {
                Id = 1,
                CriadoPor = "",
                CriadoRF = "",
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                ProfessorRf = "",
                TipoCalendarioId = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 26),
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
                NumeroAula = 1,
                AulaId = 1
            });

            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaAlunoPorTurmaEMesUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaAlunoMensal("1", 4);
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacoes = ObterTodos<Dominio.ConsolidacaoFrequenciaAlunoMensal>();

            consolidacoes.ShouldNotBeEmpty();

            consolidacoes.Count.ShouldBe(1);
            consolidacoes.FirstOrDefault().Mes.ShouldBe(4);
            consolidacoes.FirstOrDefault().Percentual.ShouldBe(100);
            consolidacoes.FirstOrDefault().QuantidadeAulas.ShouldBe(1);
            consolidacoes.FirstOrDefault().QuantidadeAusencias.ShouldBe(0);
            consolidacoes.FirstOrDefault().QuantidadeCompensacoes.ShouldBe(0);
        }

        [Fact]
        public async Task Deve_gravar_frequencia_consolidada_mes_4_e_50_de_percentual()
        {
            await CriarItensBasicos();

            await InserirNaBase(new Dominio.Aula
            {
                Id = 1,
                CriadoPor = "",
                CriadoRF = "",
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                ProfessorRf = "",
                TipoCalendarioId = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 26),
                Quantidade = 2
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
                NumeroAula = 1,
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 1,
                CodigoAluno = "1",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 1,
                AulaId = 1
            });

            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaAlunoPorTurmaEMesUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaAlunoMensal("1", 4);
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacoes = ObterTodos<Dominio.ConsolidacaoFrequenciaAlunoMensal>();

            consolidacoes.ShouldNotBeEmpty();

            consolidacoes.Count.ShouldBe(1);
            consolidacoes.FirstOrDefault().Mes.ShouldBe(4);
            consolidacoes.FirstOrDefault().Percentual.ShouldBe(50);
            consolidacoes.FirstOrDefault().QuantidadeAulas.ShouldBe(2);
            consolidacoes.FirstOrDefault().QuantidadeAusencias.ShouldBe(1);
            consolidacoes.FirstOrDefault().QuantidadeCompensacoes.ShouldBe(0);
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
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                Nome = "",
                CriadoPor = "",
                CriadoRF = ""
            });
        }

        [Fact]
        public async Task Deve_alterar_apenas_1_aluno_na_consolidacao()
        {
            await CriarItensBasicos();

            await InserirNaBase(new Dominio.Aula
            {
                Id = 1,
                CriadoPor = "",
                CriadoRF = "",
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                ProfessorRf = "",
                TipoCalendarioId = 1,
                DataAula = new DateTime(2022, 04, 26),
                Quantidade = 2
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = 1,
                AulaId = 1,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = 2,
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
                NumeroAula = 1,
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 2,
                CodigoAluno = "2",
                RegistroFrequenciaId = 2,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 1,
                NumeroAula = 1,
                AulaId = 1
            });

            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaAlunoPorTurmaEMesUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaAlunoMensal("1", 4);
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));


            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 2,
                CodigoAluno = "2",
                RegistroFrequenciaId = 2,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 1,
                AulaId = 1
            });

            useCase = ServiceProvider.GetService<IConsolidarFrequenciaAlunoPorTurmaEMesUseCase>();
            mensagem = new FiltroConsolidacaoFrequenciaAlunoMensal("1", 4);
            jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacoes = ObterTodos<Dominio.ConsolidacaoFrequenciaAlunoMensal>();

            consolidacoes.ShouldNotBeEmpty();

            consolidacoes.Count.ShouldBe(2);
            consolidacoes.Where(c=> c.AlunoCodigo == "2").FirstOrDefault().Mes.ShouldBe(4);
            consolidacoes.Where(c => c.AlunoCodigo == "2").FirstOrDefault().Percentual.ShouldBe(50);
            consolidacoes.Where(c => c.AlunoCodigo == "2").FirstOrDefault().QuantidadeAusencias.ShouldBe(1);
            consolidacoes.Where(c => c.AlunoCodigo == "2").FirstOrDefault().QuantidadeCompensacoes.ShouldBe(0);
        }
    }
}
