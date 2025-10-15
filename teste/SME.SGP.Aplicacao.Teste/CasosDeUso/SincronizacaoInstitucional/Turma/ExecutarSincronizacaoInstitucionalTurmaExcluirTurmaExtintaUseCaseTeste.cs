using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.Turma
{
    public class ExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCaseTeste
    {
        private readonly Mock<IRepositorioTurma> _repositorioTurmaMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCaseTeste()
        {
            _repositorioTurmaMock = new Mock<IRepositorioTurma>();
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase(_repositorioTurmaMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Nulo_Deve_Retornar_Falso()
        {
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(null)
            };

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            _repositorioTurmaMock.Verify(r => r.ExcluirTurmaExtintaAsync(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Exclusao_Com_Sucesso_Deve_Retornar_Verdadeiro()
        {
            var filtro = new FiltroTurmaCodigoTurmaIdDto("TURMA01", 123, DateTime.Today);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            _repositorioTurmaMock.Setup(r => r.ExcluirTurmaExtintaAsync(filtro.TurmaId)).Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            _repositorioTurmaMock.Verify(r => r.ExcluirTurmaExtintaAsync(filtro.TurmaId), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Executar_Quando_Repositorio_Lanca_Excecao_Deve_Salvar_Log_E_Relancar_Excecao(bool dataInicioPeriodoNula)
        {
            var dataInicio = dataInicioPeriodoNula ? (DateTime?)null : new DateTime(2023, 1, 1);
            var filtro = new FiltroTurmaCodigoTurmaIdDto("TURMA01", 123, new DateTime(2023, 10, 26), dataInicio);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };
            var exception = new Exception("Erro de banco");

            _repositorioTurmaMock.Setup(r => r.ExcluirTurmaExtintaAsync(filtro.TurmaId)).ThrowsAsync(exception);

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(mensagemRabbit));

            _repositorioTurmaMock.Verify(r => r.ExcluirTurmaExtintaAsync(filtro.TurmaId), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                    cmd.Contexto == LogContexto.SincronizacaoInstitucional &&
                    cmd.Nivel == LogNivel.Negocio &&
                    cmd.Observacao == exception.Message &&
                    cmd.ExcecaoInterna == "" &&
                    cmd.Mensagem.Contains(filtro.TurmaCodigo) &&
                    cmd.Mensagem.Contains("dt atualiz.") &&
                    (!dataInicioPeriodoNula ? cmd.Mensagem.Contains("dt início período:") : !cmd.Mensagem.Contains("dt início período"))),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
