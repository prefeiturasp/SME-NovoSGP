using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaDiarioBordo
{
    public class TratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase _useCase;

        public TratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new TratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Lancar_Excecao()
        {
            var filtro = new FiltroPendenciaDiarioBordoTurmaAulaDto { CodigoTurma = "TURMA-INEXISTENTE" };
            var mensagem = CriarMensagemRabbit(filtro);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new Turma { Id = 0 });

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagem));
            excecao.Message.Should().Be(MensagemAcompanhamentoTurma.TURMA_NAO_ENCONTRADA);
        }

        [Fact]
        public async Task Executar_Quando_Tipo_Escola_Ignorado_Deve_Retornar_False()
        {
            var filtro = new FiltroPendenciaDiarioBordoTurmaAulaDto { CodigoTurma = "TURMA-CEI" };
            var mensagem = CriarMensagemRabbit(filtro);
            ConfigurarMocksIniciais(new Turma { Id = 1, Ue = new Ue { CodigoUe = "UE-CEI" } });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task Executar_Quando_Pendencia_Nao_Existe_Deve_Criar_Nova_Pendencia()
        {
            var filtro = CriarFiltroDtoComAulas(1);
            var mensagem = CriarMensagemRabbit(filtro);
            long novaPendenciaId = 999;
            ConfigurarMocksIniciais(new Turma { Id = 1, Ue = new Ue { CodigoUe = "UE-EMEF" } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0);
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPendenciaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(novaPendenciaId);

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPendenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarPendenciaDiarioBordoCommand>(c => c.PendenciaId == novaPendenciaId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Pendencia_Ja_Existe_Deve_Reutilizar_Pendencia()
        {
            var filtro = CriarFiltroDtoComAulas(1);
            var mensagem = CriarMensagemRabbit(filtro);
            long pendenciaExistenteId = 888;
            ConfigurarMocksIniciais(new Turma { Id = 1, Ue = new Ue { CodigoUe = "UE-EMEF" } });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(pendenciaExistenteId);

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPendenciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarPendenciaDiarioBordoCommand>(c => c.PendenciaId == pendenciaExistenteId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Multiplas_Aulas_Iguais_Deve_Usar_Cache_Interno()
        {
            var filtro = CriarFiltroDtoComAulas(2);
            var mensagem = CriarMensagemRabbit(filtro);
            long pendenciaId = 777;
            ConfigurarMocksIniciais(new Turma { Id = 1, Ue = new Ue { CodigoUe = "UE-EMEF" } });

            _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(pendenciaId);

            var resultado = await _useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPendenciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarPendenciaDiarioBordoCommand>(c => c.PendenciaId == pendenciaId), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        private MensagemRabbit CriarMensagemRabbit(object payload)
        {
            return new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(payload) };
        }

        private void ConfigurarMocksIniciais(Turma turma)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoEscolaPorCodigoUEQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Dominio.TipoEscola.EMEF);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);
        }

        private FiltroPendenciaDiarioBordoTurmaAulaDto CriarFiltroDtoComAulas(int quantidade)
        {
            var aulas = new List<AulaProfessorComponenteDto>();
            for (int i = 1; i <= quantidade; i++)
            {
                aulas.Add(new AulaProfessorComponenteDto
                {
                    AulaId = i,
                    ComponenteCurricularId = 10,
                    ProfessorRf = "1234567",
                    PeriodoEscolarId = 20,
                    DescricaoComponenteCurricular = "Matemática"
                });
            }

            return new FiltroPendenciaDiarioBordoTurmaAulaDto
            {
                CodigoTurma = "TURMA-VALIDA",
                NomeEscola = "ESCOLA TESTE",
                TurmaComModalidade = "1º ANO - EMEF",
                AulasProfessoresComponentesCurriculares = aulas
            };
        }
    }
}
