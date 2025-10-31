using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiarioBordoPendenciaDevolutiva
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConsultasTurma> _consultasTurmaMock;
        private readonly Mock<IRepositorioPendenciaDevolutiva> _repositorioPendenciaDevolutivaMock;
        private readonly ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase _useCase;

        public ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _consultasTurmaMock = new Mock<IConsultasTurma>();
            _repositorioPendenciaDevolutivaMock = new Mock<IRepositorioPendenciaDevolutiva>();

            _useCase = new ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase(
                _mediatorMock.Object,
                _consultasTurmaMock.Object,
                _repositorioPendenciaDevolutivaMock.Object
            );
        }

        [Fact]
        public async Task Executar_Deve_Ignorar_Se_Pendencia_Ja_Existe()
        {
            var filtro = new FiltroDiarioBordoPendenciaDevolutivaDto(2025, ueCodigo: "000001", turmaId: 10);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _repositorioPendenciaDevolutivaMock
                .Setup(r => r.ObterCodigoComponenteComDiarioBordoSemDevolutiva(10, "000001"))
                .ReturnsAsync(new List<string> { "99" });

            _repositorioPendenciaDevolutivaMock
                .Setup(r => r.ObterPendenciasDevolutivaPorTurmaComponente(10, 99))
                .ReturnsAsync(new List<SME.SGP.Dominio.PendenciaDevolutiva> { new SME.SGP.Dominio.PendenciaDevolutiva() });

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(x => x.Send(It.IsAny<ExistePendenciaDiarioBordoQuery>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Gerar_Pendencia_Se_Nao_Existir_Pendencia_E_Mas_Houver_Diario_Sem_Devolutiva()
        {
            var filtro = new FiltroDiarioBordoPendenciaDevolutivaDto(2025, ueCodigo: "000001", turmaId: 10);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));
            var componente = "99";
            var turmaId = 10L;
            var ueId = 123L;
            var pendenciaGeradaId = 999L;
            var componenteIdLong = long.Parse(componente);

            _repositorioPendenciaDevolutivaMock
                .Setup(r => r.ObterCodigoComponenteComDiarioBordoSemDevolutiva(turmaId, filtro.UeCodigo))
                .ReturnsAsync(new List<string> { componente });

            _repositorioPendenciaDevolutivaMock
                .Setup(r => r.ObterPendenciasDevolutivaPorTurmaComponente(turmaId, componenteIdLong))
                .ReturnsAsync(new List<SME.SGP.Dominio.PendenciaDevolutiva>());

            _mediatorMock
                .Setup(m => m.Send(It.Is<ExistePendenciaDiarioBordoQuery>(q => q.TurmaId == turmaId && q.ComponenteCodigo == componente), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _consultasTurmaMock
                .Setup(r => r.ObterComUeDrePorId(turmaId))
                .ReturnsAsync(new Turma
                {
                    Id = turmaId,
                    Nome = "Turma Teste",
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Ue = new Ue
                    {
                        Id = ueId,
                        Nome = "EMEF Teste",
                        TipoEscola = Dominio.TipoEscola.EMEF,
                        Dre = new SME.SGP.Dominio.Dre { Id = 1, Nome = "DRE Ipiranga", Abreviacao = "DRE-IP" }
                    }
                });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDescricaoComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Matemática");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<long>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendenciaGeradaId);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<long>>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());

            _repositorioPendenciaDevolutivaMock.Verify(m => m.Salvar(It.Is<SME.SGP.Dominio.PendenciaDevolutiva>(p =>
                p.ComponenteCurricularId == componenteIdLong &&
                p.TurmaId == turmaId &&
                p.PedenciaId == pendenciaGeradaId
            )), Times.Once);
        }

        [Fact]
        public async Task Executar_Nao_Gera_Pendencia_Se_Nao_Houver_Diario_Sem_Devolutiva()
        {
            var filtro = new FiltroDiarioBordoPendenciaDevolutivaDto(2025, ueCodigo: "000001", turmaId: 10);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _repositorioPendenciaDevolutivaMock
                .Setup(r => r.ObterCodigoComponenteComDiarioBordoSemDevolutiva(10, "000001"))
                .ReturnsAsync(new List<string> { "99" });

            _repositorioPendenciaDevolutivaMock
                .Setup(r => r.ObterPendenciasDevolutivaPorTurmaComponente(10, 99))
                .ReturnsAsync(new List<SME.SGP.Dominio.PendenciaDevolutiva>());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ExistePendenciaDiarioBordoQuery>(), default))
                .ReturnsAsync(false);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPendenciaCommand>(), default), Times.Never);
            _repositorioPendenciaDevolutivaMock.Verify(m => m.Salvar(It.IsAny<SME.SGP.Dominio.PendenciaDevolutiva>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Logar_Erro_Em_Caso_De_Excecao()
        {
            var mensagem = new MensagemRabbit(new FiltroDiarioBordoPendenciaDevolutivaDto(2025, ueCodigo: "000001", turmaId: 10));

            _repositorioPendenciaDevolutivaMock
                .Setup(r => r.ObterCodigoComponenteComDiarioBordoSemDevolutiva(It.IsAny<long>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("erro simulado"));

            var resultado = await _useCase.Executar(mensagem);

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c =>
                c.Mensagem.Contains("Não foi possível executar a verificação")
                && c.Contexto == LogContexto.Devolutivas
            ), default), Times.Once);
        }
    }
}
