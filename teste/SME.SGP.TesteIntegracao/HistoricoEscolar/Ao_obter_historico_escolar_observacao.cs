using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra.Excecoes;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.HistoricoEscolar
{
    public class Ao_obter_historico_escolar_observacao : TesteBase
    {
        public Ao_obter_historico_escolar_observacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_obter_observacao_complementar_historico_escolar()
        {
            var historicoEscolarObservacao = new Dominio.HistoricoEscolarObservacao()
            {
                AlunoCodigo = "123456",
                Observacao = "observação complementar histórico aluno",
                CriadoPor = "Teste",
                CriadoRF = "123456"
            };

            await InserirNaBase(historicoEscolarObservacao);

            var obterHistoricoEscolarObservacaoUseCase = ServiceProvider.GetService<IObterHistoricoEscolarObservacaoUseCase>();
            var historicoEscolarObservacaoDto = await obterHistoricoEscolarObservacaoUseCase.Executar(historicoEscolarObservacao.AlunoCodigo);

            historicoEscolarObservacaoDto.Observacao.ShouldNotBeEmpty();
            historicoEscolarObservacaoDto.Observacao.ShouldBe(historicoEscolarObservacao.Observacao);
        }

        [Fact]
        public async Task Nao_deve_obter_observacao_complementar_historico_escolar()
        {
            var alunoCodigo = "654321";
            var obterHistoricoEscolarObservacaoUseCase = ServiceProvider.GetService<IObterHistoricoEscolarObservacaoUseCase>();
            var historicoEscolarObservacaoDto = await obterHistoricoEscolarObservacaoUseCase.Executar(alunoCodigo);

            historicoEscolarObservacaoDto.ShouldBeNull();
        }

        [Fact]
        public async Task Excecao_validacao_obter_observacao_complementar_historico_escolar()
        {
            string alunoCodigo = null;
            var obterHistoricoEscolarObservacaoUseCase = ServiceProvider.GetService<IObterHistoricoEscolarObservacaoUseCase>();

            var ex = await Should.ThrowAsync<ValidacaoException>(() => obterHistoricoEscolarObservacaoUseCase.Executar(alunoCodigo));

            ex.Erros.FirstOrDefault().ErrorMessage.ShouldBe("O Código do Aluno deve ser informado.");
        }
    }
}
