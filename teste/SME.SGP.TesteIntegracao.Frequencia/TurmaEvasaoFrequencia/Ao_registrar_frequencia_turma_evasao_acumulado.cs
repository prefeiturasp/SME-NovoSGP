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
    public class Ao_registrar_frequencia_turma_evasao_acumulado : TesteBase
    {
        public Ao_registrar_frequencia_turma_evasao_acumulado(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoEvasaoFrequenciaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFrequenciaGeralPorAlunosTurmaQuery, IEnumerable<Dominio.FrequenciaAluno>>), typeof(ObterFrequenciaGeralPorAlunosTurmaEvasaoFrequenciaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_registrar_a_quantidade_de_alunos_com_frequencia_abaixo_50_porcento_e_sem_frequencia_por_turma_acumulado()
        {
            await CriarItensBasicos();
            var useCase = ServiceProvider.GetService<IConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase>();
            var mensagem = new FiltroConsolidacaoFrequenciaTurmaEvasaoAcumulado(DateTimeExtension.HorarioBrasilia().Year, 1);
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacoes = ObterTodos<Dominio.FrequenciaTurmaEvasao>();
            consolidacoes.ShouldNotBeEmpty();
            consolidacoes.Count.ShouldBe(1);
            consolidacoes.FirstOrDefault().Mes.ShouldBe(0);
            consolidacoes.FirstOrDefault().QuantidadeAlunosAbaixo50Porcento.ShouldBe(4);
            consolidacoes.FirstOrDefault().QuantidadeAlunos0Porcento.ShouldBe(2);

            var consolidacoesAluno = ObterTodos<Dominio.FrequenciaTurmaEvasaoAluno>();
            consolidacoesAluno.ShouldNotBeEmpty();
            consolidacoesAluno.Count.ShouldBe(4);
            consolidacoesAluno.Any(ca => ca.AlunoCodigo.Equals("3") && ca.PercentualFrequencia.Equals(40)).ShouldBeTrue();
            consolidacoesAluno.Any(ca => ca.AlunoCodigo.Equals("4") && ca.PercentualFrequencia.Equals(30)).ShouldBeTrue();
            consolidacoesAluno.Any(ca => ca.AlunoCodigo.Equals("5") && ca.PercentualFrequencia.Equals(0)).ShouldBeTrue();
            consolidacoesAluno.Any(ca => ca.AlunoCodigo.Equals("6") && ca.PercentualFrequencia.Equals(0)).ShouldBeTrue();
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
