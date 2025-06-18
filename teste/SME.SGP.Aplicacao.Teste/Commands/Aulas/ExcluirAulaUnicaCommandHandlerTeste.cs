using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.Aulas
{

    public class ExcluirAulaUnicaCommandHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioAula> _repositorioAulaMock;
        private readonly Mock<IRepositorioAnotacaoFrequenciaAlunoConsulta> _repositorioAnotacaoFrequenciaAlunoMock;
        private readonly Mock<IRepositorioDiarioBordo> _repositorioDiarioBordoMock;
        private readonly Mock<IRepositorioPlanoAula> _repositorioPlanoAulaMock;
        private readonly ExcluirAulaUnicaCommandHandler _commandHandler;
        private readonly Faker _faker;

        public ExcluirAulaUnicaCommandHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioAulaMock = new Mock<IRepositorioAula>();
            _repositorioAnotacaoFrequenciaAlunoMock = new Mock<IRepositorioAnotacaoFrequenciaAlunoConsulta>();
            _repositorioDiarioBordoMock = new Mock<IRepositorioDiarioBordo>();
            _repositorioPlanoAulaMock = new Mock<IRepositorioPlanoAula>();
            _faker = new Faker("pt_BR");

            _commandHandler = new ExcluirAulaUnicaCommandHandler(
                _mediatorMock.Object,
                _repositorioAulaMock.Object,
                _repositorioAnotacaoFrequenciaAlunoMock.Object,
                _repositorioDiarioBordoMock.Object,
                _repositorioPlanoAulaMock.Object);
        }

        private ExcluirAulaUnicaCommand CriarComandoFake(bool ehGestor)
        {
            var usuario = new Usuario { CodigoRf = "123456", Nome = "Teste" };
            // A lógica de EhGestorEscolar depende do PerfilAtual
            usuario.DefinirPerfilAtual(ehGestor ? Perfis.PERFIL_DIRETOR : Perfis.PERFIL_PROFESSOR);

            var aula = new Aula
            {
                Id = _faker.Random.Long(1, 1000),
                TurmaId = _faker.Random.AlphaNumeric(10),
                DisciplinaId = _faker.Random.Long(1, 100).ToString(),
                DataAula = DateTime.Today,
                WorkflowAprovacaoId = _faker.Random.Long(1, 500)
            };

            return new ExcluirAulaUnicaCommand(usuario, aula);
        }

        [Fact(DisplayName = "Deve lançar exceção quando a aula possuir avaliação")]
        public async Task Handle_QuandoAulaPossuiAvaliacao_DeveLancarNegocioException()
        {
            // Organização
            var comando = CriarComandoFake(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<AulaPossuiAvaliacaoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Ação e Verificação
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _commandHandler.Handle(comando, default));
            excecao.Message.Should().Be("Aula com avaliação vinculada. Para excluir esta aula primeiro deverá ser excluída a avaliação.");
        }

        [Fact(DisplayName = "Deve lançar exceção quando a validação de componentes do professor falhar")]
        public async Task Handle_QuandoValidacaoProfessorFalhar_DeveLancarNegocioException()
        {
            // Organização
            var comando = CriarComandoFake(false); // Usuário não é gestor
            _mediatorMock.Setup(m => m.Send(It.IsAny<AulaPossuiAvaliacaoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ValidarComponentesDoProfessorCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((false, "Professor não pode excluir a aula."));

            // Ação e Verificação
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _commandHandler.Handle(comando, default));
            excecao.Message.Should().Be("Professor não pode excluir a aula.");
        }

        [Fact(DisplayName = "Deve executar todas as operações de exclusão em caso de sucesso")]
        public async Task Handle_QuandoValido_DeveExecutarTodasAsOperacoesDeExclusao()
        {
            // Organização
            var comando = CriarComandoFake(true);
            Aula aulaCapturada = null;

            // Prepara retornos dos repositórios para simular arquivos existentes
            _repositorioPlanoAulaMock.Setup(r => r.ObterPlanoAulaPorAulaRegistroExcluido(comando.Aula.Id))
                .ReturnsAsync(new PlanoAula { Descricao = "arquivo1", RecuperacaoAula = "arquivo2", LicaoCasa = "arquivo3" });

            _repositorioDiarioBordoMock.Setup(r => r.ObterPorAulaId(comando.Aula.Id))
                .ReturnsAsync(new List<DiarioBordo> { new DiarioBordo { Planejamento = "arquivo4" } });

            _repositorioAnotacaoFrequenciaAlunoMock.Setup(r => r.ObterPorAulaIdRegistroExcluido(comando.Aula.Id))
                .ReturnsAsync(new List<AnotacaoFrequenciaAluno> { new AnotacaoFrequenciaAluno { Anotacao = "arquivo5" } });

            _repositorioAulaMock.Setup(r => r.SalvarAsync(It.IsAny<Aula>()))
                                .Callback<Aula>(aula => aulaCapturada = aula);

            // Ação
            var retorno = await _commandHandler.Handle(comando, default);

            // Verificação
            retorno.Should().NotBeNull();
            retorno.Mensagens.Should().Contain("Aula excluída com sucesso.");

            // Verifica se a aula foi marcada como excluída
            aulaCapturada.Should().NotBeNull();
            aulaCapturada.Excluido.Should().BeTrue();

            // Verifica publicações em fila
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgp.WorkflowAprovacaoExcluir), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaEmLoteSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            // Verifica comandos subsequentes
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RecalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            // Verifica remoção de arquivos (5 arquivos no total)
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(5));
        }

        [Fact(DisplayName = "Não deve remover arquivos quando eles não existirem")]
        public async Task Handle_QuandoNaoHouverArquivos_NaoDeveTentarRemover()
        {
            // Organização
            var comando = CriarComandoFake(true);

            // Simula repositórios retornando nulo ou lista vazia
            _repositorioPlanoAulaMock.Setup(r => r.ObterPlanoAulaPorAulaRegistroExcluido(comando.Aula.Id)).ReturnsAsync((PlanoAula)null);
            _repositorioDiarioBordoMock.Setup(r => r.ObterPorAulaId(comando.Aula.Id)).ReturnsAsync(new List<DiarioBordo>());
            _repositorioAnotacaoFrequenciaAlunoMock.Setup(r => r.ObterPorAulaIdRegistroExcluido(comando.Aula.Id)).ReturnsAsync(new List<AnotacaoFrequenciaAluno>());

            // Ação
            await _commandHandler.Handle(comando, default);

            // Verificação
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
