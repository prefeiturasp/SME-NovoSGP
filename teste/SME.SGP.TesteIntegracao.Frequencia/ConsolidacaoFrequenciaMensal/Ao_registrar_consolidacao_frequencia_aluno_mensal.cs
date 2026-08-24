using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsolidacaoDashboardFrequenciaTurma
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
                Id = 2,
                CodigoAluno = "1",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 2,
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

            await InserirNaBase(new Dominio.Turma
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year
            });

            await InserirNaBase(new Dominio.TipoCalendario
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
                NumeroAula = 2,
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

        [Fact]
        public async Task Deve_serializar_consolidacoes_da_mesma_turma_e_mes()
        {
            await using var conexao1 = new NpgsqlConnection(_collectionFixture.Database.ConnectionString);
            await using var conexao2 = new NpgsqlConnection(_collectionFixture.Database.ConnectionString);
            await conexao1.OpenAsync();
            await conexao2.OpenAsync();

            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            using var contexto1 = new SgpContext(conexao1, contextoAplicacao);
            using var contexto2 = new SgpContext(conexao2, contextoAplicacao);
            using var unitOfWork1 = new UnitOfWork(contexto1);
            using var unitOfWork2 = new UnitOfWork(contexto2);
            var repositorio1 = new SME.SGP.Dados.Repositorios.RepositorioConsolidacaoFrequenciaAlunoMensal(contexto1);
            var repositorio2 = new SME.SGP.Dados.Repositorios.RepositorioConsolidacaoFrequenciaAlunoMensal(contexto2);

            unitOfWork1.IniciarTransacao();
            unitOfWork2.IniciarTransacao();

            await repositorio1.BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMes(1, 4);
            var segundoLock = repositorio2.BloquearConsolidacaoFrequenciaAlunoMensalPorTurmaEMes(1, 4);

            var resultadoAntesDoCommit = await Task.WhenAny(segundoLock, Task.Delay(300));
            resultadoAntesDoCommit.ShouldNotBe(segundoLock);

            unitOfWork1.PersistirTransacao();

            var resultadoDepoisDoCommit = await Task.WhenAny(segundoLock, Task.Delay(TimeSpan.FromSeconds(5)));
            resultadoDepoisDoCommit.ShouldBe(segundoLock);
            await segundoLock;

            unitOfWork2.PersistirTransacao();
        }
    }
}
