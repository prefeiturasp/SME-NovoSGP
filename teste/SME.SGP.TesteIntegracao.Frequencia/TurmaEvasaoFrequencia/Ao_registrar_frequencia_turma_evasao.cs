using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Frequencia.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.FrequenciaTurmaEvasao
{
    public class Ao_registrar_frequencia_turma_evasao : TesteBase
    {
        public Ao_registrar_frequencia_turma_evasao(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaEvasaoFrequenciaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_registrar_a_quantidade_de_alunos_com_frequencia_abaixo_50_porcento_e_sem_frequencia_por_turma_e_mes()
        {
            await CriarItensBasicos();
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaTurmaEvasaoUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaTurmaEvasao(1, 5);
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacoes = ObterTodos<Dominio.FrequenciaTurmaEvasao>();
            consolidacoes.ShouldNotBeEmpty();
            consolidacoes.Count.ShouldBe(1);
            consolidacoes.FirstOrDefault().Mes.ShouldBe(5);
            consolidacoes.FirstOrDefault().QuantidadeAlunosAbaixo50Porcento.ShouldBe(2);
            consolidacoes.FirstOrDefault().QuantidadeAlunos0Porcento.ShouldBe(2);

            var consolidacoesAluno = ObterTodos<Dominio.FrequenciaTurmaEvasaoAluno>();
            consolidacoesAluno.ShouldNotBeEmpty();
            consolidacoesAluno.Count.ShouldBe(4);
            consolidacoesAluno.Any(ca => ca.AlunoCodigo.Equals("3") && ca.PercentualFrequencia.Equals(40)).ShouldBeTrue();
            consolidacoesAluno.Any(ca => ca.AlunoCodigo.Equals("4") && ca.PercentualFrequencia.Equals(30)).ShouldBeTrue();
            consolidacoesAluno.Any(ca => ca.AlunoCodigo.Equals("5") && ca.PercentualFrequencia.Equals(0)).ShouldBeTrue();
            consolidacoesAluno.Any(ca => ca.AlunoCodigo.Equals("6") && ca.PercentualFrequencia.Equals(0)).ShouldBeTrue();
        }

        private async Task CriarRegistrosConsolidacaoFrequenciaAlunoMensal()
        {
            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "1",
                Mes = 5,
                Percentual = 50,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 5,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "2",
                Mes = 5,
                Percentual = 80,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 8,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "3",
                Mes = 5,
                Percentual = 40,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 6,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "4",
                Mes = 5,
                Percentual = 30,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 7,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "5",
                Mes = 5,
                Percentual = 0,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 10,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                Id = 1,
                TurmaId = 1,
                AlunoCodigo = "6",
                Mes = 5,
                Percentual = 0,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 10,
                QuantidadeCompensacoes = 0
            });
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
