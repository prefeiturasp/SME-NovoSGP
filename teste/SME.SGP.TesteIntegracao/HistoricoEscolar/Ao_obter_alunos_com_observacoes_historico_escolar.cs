using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.HistoricoEscolar.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.HistoricoEscolar
{
    public class Ao_obter_alunos_com_observacoes_historico_escolar : TesteBase
    {
        private const string CODIGO_TURMA = "1";
        public Ao_obter_alunos_com_observacoes_historico_escolar(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }


        [Fact]
        public async Task Deve_obter_alunos_com_todas_observacoes_historico_escolar()
        {
            await InserirHistoricoObservacao("1", "Observacao aluno 1");
            await InserirHistoricoObservacao("2", "Observacao aluno 2");
            await InserirHistoricoObservacao("3", "Observacao aluno 3");
            await InserirHistoricoObservacao("4", "Observacao aluno 4");
            await InserirHistoricoObservacao("5", "Observacao aluno 5");

            var useCase = ServiceProvider.GetService<IObterObservacoesDosAlunosNoHistoricoEscolarUseCase>();
            var paginacaoResultado = await useCase.Executar(CODIGO_TURMA);

            foreach(var item in paginacaoResultado.Items) 
            {
                item.Observacao.ShouldNotBeEmpty();    
            }
        }

        [Fact]
        public async Task Deve_obter_alunos_uma_observacao_historico_escolar()
        {
            await InserirHistoricoObservacao("1", "Observacao aluno 1");

            var useCase = ServiceProvider.GetService<IObterObservacoesDosAlunosNoHistoricoEscolarUseCase>();
            var paginacaoResultado = await useCase.Executar(CODIGO_TURMA);

            var aluno1 = paginacaoResultado.Items.ToList().Find(aluno => aluno.Codigo == "1");
            aluno1.ShouldNotBeNull();
            aluno1.Observacao.ShouldNotBeEmpty();
            aluno1.Observacao.ShouldBe("Observacao aluno 1");

            foreach (var item in paginacaoResultado.Items.Where(aluno => aluno.Codigo != "1"))
            {
                item.Observacao.ShouldBeEmpty();
            }
        }

        [Fact]
        public async Task Deve_obter_alunos_com_nenhuma_observacao_historico_escolar()
        {
            var useCase = ServiceProvider.GetService<IObterObservacoesDosAlunosNoHistoricoEscolarUseCase>();
            var paginacaoResultado = await useCase.Executar(CODIGO_TURMA);

            foreach (var item in paginacaoResultado.Items)
            {
                item.Observacao.ShouldBeEmpty();
            }
        }

        private async Task InserirHistoricoObservacao(string codigoAluno, string observacao)
        {
            var historicoEscolarObservacao = new Dominio.HistoricoEscolarObservacao()
            {
                AlunoCodigo = codigoAluno,
                Observacao = observacao,
                CriadoPor = "Teste",
                CriadoRF = "123"
            };

            await InserirNaBase(historicoEscolarObservacao);
        }
    }
}
