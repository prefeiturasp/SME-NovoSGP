using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.ObterSemestresPorAnoLetivoModalidadeEUeCodigo
{
    public class ObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase useCase;

        public ObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Semestres_Quando_Usuario_Existe_E_Query_Retorna_Resultado()
        {
            var usuario = new Usuario { Login = "usuario.teste", PerfilAtual = Guid.NewGuid() };
            var semestres = new List<int> { 1, 2 };

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.Is<ObterSemestresPorAnoLetivoModalidadeEUeCodigoQuery>(q =>
                q.Login == usuario.Login &&
                q.Perfil == usuario.PerfilAtual &&
                q.Modalidade == 1 &&
                q.AnoLetivo == 2025 &&
                q.ConsideraHistorico == false &&
                q.UeCodigo == "1234"), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(semestres);

            var resultado = await useCase.Executar(false, 1, 2025, "1234");

            Assert.NotNull(resultado);
            Assert.Equal(semestres, resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_Encontrado()
        {
            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Usuario)null);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() =>
                useCase.Executar(true, 1, 2024, "9999"));

            Assert.Equal("Usuário não encontrado", excecao.Message);
        }

        [Fact]
        public async Task Deve_Retornar_Default_Quando_Semestres_Forem_Nulos_Ou_Vazios()
        {
            var usuario = new Usuario { Login = "usuario.vazio", PerfilAtual = Guid.NewGuid() };

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSemestresPorAnoLetivoModalidadeEUeCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IEnumerable<int>)null); 

            var resultado = await useCase.Executar(true, 3, 2023, "8888");

            Assert.Null(resultado);
        }
    }
}
