using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Usuarios
{
    public class ObterListaSituacoesUsuarioUseCaseTeste
    {
        private readonly IObterListaSituacoesUsuarioUseCase _useCase;

        public ObterListaSituacoesUsuarioUseCaseTeste()
        {
            _useCase = new ObterListaSituacoesUsuarioUseCase();
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Retornar_Lista_Completa_De_Situacoes()
        {
            var situacoesEsperadas = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>((int)SituacaoUsuario.Ativo, SituacaoUsuario.Ativo.Name()),
                new KeyValuePair<int, string>((int)SituacaoUsuario.Excluido, SituacaoUsuario.Excluido.Name()),
                new KeyValuePair<int, string>((int)SituacaoUsuario.SenhaExpirada, SituacaoUsuario.SenhaExpirada.Name()),
                new KeyValuePair<int, string>((int)SituacaoUsuario.Excluido, SituacaoUsuario.Excluido.Name()),
                new KeyValuePair<int, string>((int)SituacaoUsuario.SenhaExpirada, SituacaoUsuario.SenhaExpirada.Name())
            };

            var resultado = await _useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Equal(situacoesEsperadas.Count, resultado.Count());
            Assert.True(situacoesEsperadas.All(esperada => resultado.Any(r => r.Key == esperada.Key && r.Value == esperada.Value)));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Retornar_Tipo_Correto()
        {
            var resultado = await _useCase.Executar();

            Assert.IsAssignableFrom<IEnumerable<KeyValuePair<int, string>>>(resultado);
        }
    }
}
