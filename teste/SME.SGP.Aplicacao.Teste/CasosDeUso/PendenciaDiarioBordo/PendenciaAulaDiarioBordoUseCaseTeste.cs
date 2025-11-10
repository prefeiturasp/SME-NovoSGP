using Bogus;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaDiarioBordo
{
    public class PendenciaAulaDiarioBordoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PendenciaAulaDiarioBordoUseCase _useCase;
        private readonly Faker _faker;

        public PendenciaAulaDiarioBordoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PendenciaAulaDiarioBordoUseCase(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve publicar na fila para cada turma encontrada")]
        public async Task Executar_DevePublicarNaFilaParaCadaTurmaEncontrada()
        {
            // Organização
            var filtro = new DreUeDto(
                dreId: _faker.Random.Long(1, 10),
                codigoUe: _faker.Random.AlphaNumeric(10));

            var turmas = new List<TurmaDTO>
            {
                new TurmaDTO { TurmaId = _faker.Random.Long(1, 1000), TurmaCodigo = _faker.Random.AlphaNumeric(10) },
                new TurmaDTO { TurmaId = _faker.Random.Long(1, 1000), TurmaCodigo = _faker.Random.AlphaNumeric(10) }
            };

            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));
            var anoLetivoCorrente = DateTime.Now.Year;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasInfantilPorUEQuery>(q =>
                                     q.UeCodigo == filtro.CodigoUe && q.AnoLetivo == anoLetivoCorrente),
                                     It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turmas);

            // Ação
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Verificação
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            foreach (var turma in turmas)
            {
                _mediatorMock.Verify(m => m.Send(
                    It.Is<PublicarFilaSgpCommand>(c =>
                        c.Rota == RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordoTurma &&
                        c.Filtros.ToString() == turma.TurmaCodigo),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Fact(DisplayName = "Não deve publicar na fila quando nenhuma turma for encontrada")]
        public async Task Executar_NaoDevePublicarNaFilaQuandoNenhumaTurmaForEncontrada()
        {
            // Organização
            var filtro = new DreUeDto(
                dreId: _faker.Random.Long(1, 10),
                codigoUe: _faker.Random.AlphaNumeric(10));

            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TurmaDTO>()); // Retorna lista vazia

            // Ação
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Verificação
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasInfantilPorUEQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Não deve processar quando a mensagem for inválida ou o filtro for nulo")]
        public async Task Executar_NaoDeveProcessarQuandoMensagemForInvalida()
        {
            // Organização
            // Uma mensagem com "null" como conteúdo resultará em um filtro nulo após a desserialização.
            var mensagemRabbit = new MensagemRabbit("null");

            // Ação
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Verificação
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<IEnumerable<TurmaDTO>>>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
