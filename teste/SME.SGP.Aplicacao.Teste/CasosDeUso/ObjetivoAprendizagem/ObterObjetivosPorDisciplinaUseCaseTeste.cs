using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ObjetivoAprendizagem
{
    public class ObterObjetivosPorDisciplinaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IObterObjetivosPorDisciplinaUseCase _useCase;

        public ObterObjetivosPorDisciplinaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterObjetivosPorDisciplinaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Regencia_True_E_Usuario_Nao_Eh_Cj_Deve_Filtrar_Regencia()
        {
            var usuario = new Usuario { PerfilAtual = Perfis.PERFIL_PROFESSOR };
            ConfigurarMocksIniciais(usuario);

            await _useCase.Executar(DateTime.Now, 1, 2, 3, true);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterObjetivosPlanoDisciplinaQuery>(c => c.FiltrarSomenteRegencia), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Regencia_False_Deve_Nao_Filtrar_Regencia()
        {
            var usuario = new Usuario { PerfilAtual = Perfis.PERFIL_PROFESSOR };
            ConfigurarMocksIniciais(usuario);

            await _useCase.Executar(DateTime.Now, 1, 2, 3, false);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterObjetivosPlanoDisciplinaQuery>(c => !c.FiltrarSomenteRegencia), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Eh_Cj_Deve_Nao_Filtrar_Regencia()
        {
            var usuario = new Usuario { PerfilAtual = Perfis.PERFIL_CJ };
            ConfigurarMocksIniciais(usuario);

            await _useCase.Executar(DateTime.Now, 1, 2, 3, true);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterObjetivosPlanoDisciplinaQuery>(c => !c.FiltrarSomenteRegencia), It.IsAny<CancellationToken>()), Times.Once);
        }

        private void ConfigurarMocksIniciais(Usuario usuario)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Turma());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestreAtualPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);
        }
    }
}
