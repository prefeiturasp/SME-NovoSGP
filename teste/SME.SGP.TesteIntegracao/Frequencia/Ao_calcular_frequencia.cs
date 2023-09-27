using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Frequencia.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.FrequenciaAluno
{
    public class Ao_calcular_frequencia : TesteBase
    {
        public Ao_calcular_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Frequência - Deve retornar false se a mensagem estiver vazia quando recalcula a frequência")]
        public async Task Deve_retornar_false_se_a_mensagem_estiver_vazia()
        {
            var useCase = ServiceProvider.GetService<ICalculoFrequenciaTurmaDisciplinaUseCase>();

            var retorno = await useCase.Executar(new MensagemRabbit(""));

            retorno.ShouldBeFalse();
        }

        [Fact(DisplayName = "Frequência - Deve gravar 100 de percentual de frequencia para um aluno quando recalcula a frequência")]
        public async Task Deve_gravar_100_de_percentual_de_frequencia_para_um_aluno()
        {
            var useCase = ServiceProvider.GetService<ICalculoFrequenciaTurmaDisciplinaUseCase>();

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
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15),
                Quantidade = 1
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = 1,
                AulaId = 1,
                CriadoPor = "",
                CriadoRF = "",

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

            var mensagem = new CalcularFrequenciaPorTurmaCommand(new List<string> { "1" }, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15), "1", "1");
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var frequencias = ObterTodos<Dominio.FrequenciaAluno>();

            frequencias.ShouldNotBeEmpty();

            frequencias.Count.ShouldBe(2);
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral).ShouldNotBeNull();
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).ShouldNotBeNull();
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).PercentualFrequencia.ShouldBe(100);
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.Geral).PercentualFrequencia.ShouldBe(100);

        }

        [Fact(DisplayName = "Frequência - Deve gravar 50 de percentual de frequencia para um aluno quando recalcula a frequência")]
        public async Task Deve_gravar_50_de_percentual_de_frequencia_discplina_para_um_aluno()
        {
            var useCase = ServiceProvider.GetService<ICalculoFrequenciaTurmaDisciplinaUseCase>();
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
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15),
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

            var mensagem = new CalcularFrequenciaPorTurmaCommand(new List<string> { "1" }, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15), "1", "1");
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var frequencias = ObterTodos<Dominio.FrequenciaAluno>();

            frequencias.ShouldNotBeEmpty();

            frequencias.Count.ShouldBe(2);
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral).ShouldNotBeNull();
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).ShouldNotBeNull();
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).PercentualFrequencia.ShouldBe(50);
        }

        [Fact(DisplayName = "Frequência - Deve gravar 50 de percentual de frequencia discplina mesmo com duplicidade de ausencia na mesma aula quando recalcula a frequência")]
        public async Task Deve_gravar_50_de_percentual_de_frequencia_discplina_mesmo_com_duplicidade_de_ausencia_na_mesma_aula()
        {
            var useCase = ServiceProvider.GetService<ICalculoFrequenciaTurmaDisciplinaUseCase>();
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
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15),
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
                NumeroAula = 1,
                AulaId = 1
            });            

            var mensagem = new CalcularFrequenciaPorTurmaCommand(new List<string> { "1" }, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15), "1", "1");
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var frequencias = ObterTodos<Dominio.FrequenciaAluno>();

            frequencias.ShouldNotBeEmpty();

            frequencias.Count.ShouldBe(2);
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral).ShouldNotBeNull();
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).ShouldNotBeNull();
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).PercentualFrequencia.ShouldBe(50);
        }

        [Fact]        
        public async Task Deve_gravar_50_de_percentual_de_frequencia_discplina_para_um_aluno_e_75_para_outro()
        {
            var useCase = ServiceProvider.GetService<ICalculoFrequenciaTurmaDisciplinaUseCase>();
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
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15),
                Quantidade = 2
            });

            await InserirNaBase(new Dominio.Aula
            {
                Id = 2,
                CriadoPor = "",
                CriadoRF = "",
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                ProfessorRf = "",
                TipoCalendarioId = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 16),
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
                AulaId = 2,
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
                Valor = 1,
                NumeroAula = 2,
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 3,
                CodigoAluno = "1",
                RegistroFrequenciaId = 2,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 1,
                AulaId = 2
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 4,
                CodigoAluno = "1",
                RegistroFrequenciaId = 2,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 2,
                AulaId = 2
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 5,
                CodigoAluno = "2",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 1,
                NumeroAula = 1,
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 6,
                CodigoAluno = "2",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 1,
                NumeroAula = 2,
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 7,
                CodigoAluno = "2",
                RegistroFrequenciaId = 2,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 1,
                AulaId = 2
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 8,
                CodigoAluno = "2",
                RegistroFrequenciaId = 2,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 3,
                NumeroAula = 2,
                AulaId = 2
            });

            var mensagem = new CalcularFrequenciaPorTurmaCommand(new List<string> { "1" }, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15), "1", "1");
            var jsonMensagem = JsonSerializer.Serialize(mensagem);
            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            mensagem = new CalcularFrequenciaPorTurmaCommand(new List<string> { "2" }, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 16), "1", "1");
            jsonMensagem = JsonSerializer.Serialize(mensagem);
            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var frequencias = ObterTodos<Dominio.FrequenciaAluno>();

            frequencias.ShouldNotBeEmpty();

            frequencias.Count.ShouldBe(4);
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina && x.CodigoAluno == "1").PercentualFrequencia.ShouldBe(50);
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina && x.CodigoAluno == "2").PercentualFrequencia.ShouldBe(75);
        }

        [Fact]
        public async Task Deve_gravar_100_por_cento_para_um_aluno_com_compensacao()
        {
            var useCase = ServiceProvider.GetService<ICalculoFrequenciaTurmaDisciplinaUseCase>();
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
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15),
                Quantidade = 3
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = 1,
                AulaId = 1,
                CriadoPor = "",
                CriadoRF = "",

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
                NumeroAula = 1,
                AulaId = 1
            });

            await InserirNaBase(new CompensacaoAusencia
            {
                Id = 1,
                CriadoPor = "",
                CriadoRF = "",
                TurmaId = 1,
                DisciplinaId = "1",
                Bimestre = 3,
                Nome = "",
                Descricao = ""
            });

            await InserirNaBase(new CompensacaoAusenciaAluno
            {
                Id = 1,
                CodigoAluno = "1",
                CriadoPor = "",
                CriadoRF = "",
                QuantidadeFaltasCompensadas = 1,
                CompensacaoAusenciaId = 1
            });

            var mensagem = new CalcularFrequenciaPorTurmaCommand(new List<string> { "1" }, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15), "1", "1");
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var frequencias = ObterTodos<Dominio.FrequenciaAluno>();

            frequencias.ShouldNotBeEmpty();

            frequencias.Count.ShouldBe(2);
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral).ShouldNotBeNull();
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).ShouldNotBeNull();
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).PercentualFrequencia.ShouldBe(100);
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.Geral).PercentualFrequencia.ShouldBe(100);

        }

        [Fact]
        public async Task Deve_gravar_100_de_percentual_de_frequencia_para_um_aluno_com_presencial_e_remoto()
        {
            var useCase = ServiceProvider.GetService<ICalculoFrequenciaTurmaDisciplinaUseCase>();

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
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15),
                Quantidade = 2
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = 1,
                AulaId = 1,
                CriadoPor = "",
                CriadoRF = "",

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
                Valor = 3,
                NumeroAula = 2,
                AulaId = 1
            });

            var mensagem = new CalcularFrequenciaPorTurmaCommand(new List<string> { "1" }, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15), "1", "1");
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var frequencias = ObterTodos<Dominio.FrequenciaAluno>();

            frequencias.ShouldNotBeEmpty();

            frequencias.Count.ShouldBe(2);
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral).ShouldNotBeNull();
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).ShouldNotBeNull();
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).PercentualFrequencia.ShouldBe(100);
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.Geral).PercentualFrequencia.ShouldBe(100);

        }

        [Fact]        
        public async Task Deve_gravar_50_de_percentual_de_frequencia_para_um_aluno_com_presencial_e_remoto()
        {
            var useCase = ServiceProvider.GetService<ICalculoFrequenciaTurmaDisciplinaUseCase>();

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
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15),
                Quantidade = 4
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = 1,
                AulaId = 1,
                CriadoPor = "",
                CriadoRF = "",

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
                Valor = 3,
                NumeroAula = 2,
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 3,
                CodigoAluno = "1",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 3,
                AulaId = 1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                Id = 4,
                CodigoAluno = "1",
                RegistroFrequenciaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                Valor = 2,
                NumeroAula = 4,
                AulaId = 1
            });

            var mensagem = new CalcularFrequenciaPorTurmaCommand(new List<string> { "1" }, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 15), "1", "1");
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var frequencias = ObterTodos<Dominio.FrequenciaAluno>();

            frequencias.ShouldNotBeEmpty();

            frequencias.Count.ShouldBe(2);
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.Geral).ShouldNotBeNull();
            frequencias.FirstOrDefault(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).ShouldNotBeNull();
            frequencias.First(x => x.Tipo == TipoFrequenciaAluno.PorDisciplina).PercentualFrequencia.ShouldBe(50);
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
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = DateTime.Now.Year
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                Nome = "",
                CriadoPor = "",
                CriadoRF = "",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                AnoLetivo = DateTime.Now.Year,
                Situacao = true
            });

            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                Bimestre = 3,
                TipoCalendarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 01),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 09, 30)
            });

        }
    }
}
