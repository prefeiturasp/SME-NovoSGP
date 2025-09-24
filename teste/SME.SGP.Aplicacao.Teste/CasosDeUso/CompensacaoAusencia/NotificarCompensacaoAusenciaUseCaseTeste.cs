using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Relatorios.HistoricoEscolar;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class NotificarCompensacaoAusenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioCompensacaoAusenciaAlunoConsulta> _repoAlunoConsultaMock;
        private readonly Mock<IRepositorioCompensacaoAusenciaAluno> _repoAlunoMock;
        private readonly Mock<IRepositorioCompensacaoAusencia> _repoCompensacaoMock;
        private readonly Mock<IRepositorioTurmaConsulta> _repoTurmaConsultaMock;
        private readonly NotificarCompensacaoAusenciaUseCase _useCase;

        public NotificarCompensacaoAusenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repoAlunoConsultaMock = new Mock<IRepositorioCompensacaoAusenciaAlunoConsulta>();
            _repoAlunoMock = new Mock<IRepositorioCompensacaoAusenciaAluno>();
            _repoCompensacaoMock = new Mock<IRepositorioCompensacaoAusencia>();
            _repoTurmaConsultaMock = new Mock<IRepositorioTurmaConsulta>();

            _useCase = new NotificarCompensacaoAusenciaUseCase(
                _mediatorMock.Object,
                _repoAlunoConsultaMock.Object,
                _repoAlunoMock.Object,
                _repoCompensacaoMock.Object,
                _repoTurmaConsultaMock.Object
            );

        }

        [Fact]
        public async Task Executar_DeveRetornarTrue_QuandoNaoHaAlunos()
        {
            // Arrange
            var compensacaoId = 123;
            var filtro = new SME.SGP.Infra.Dtos.FiltroNotificacaoCompensacaoAusenciaDto(compensacaoId);
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _repoAlunoConsultaMock.Setup(r => r.ObterPorCompensacao(compensacaoId)).ReturnsAsync(new List<CompensacaoAusenciaAluno>());

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            _repoAlunoConsultaMock.Verify(r => r.ObterPorCompensacao(compensacaoId), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRetornarTrue_QuandoTodosAlunosJaNotificadosOuSemFaltas()
        {
            // Arrange
            var compensacaoId = 123;
            var filtro = new SME.SGP.Infra.Dtos.FiltroNotificacaoCompensacaoAusenciaDto(compensacaoId);
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var alunos = new List<CompensacaoAusenciaAluno>
        {
            new CompensacaoAusenciaAluno { CodigoAluno = "A1", Notificado = true, QuantidadeFaltasCompensadas = 2 },
            new CompensacaoAusenciaAluno { CodigoAluno = "A2", Notificado = false, QuantidadeFaltasCompensadas = 0 }
        };

            _repoAlunoConsultaMock.Setup(r => r.ObterPorCompensacao(compensacaoId)).ReturnsAsync(alunos);

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            _repoAlunoConsultaMock.Verify(r => r.ObterPorCompensacao(compensacaoId), Times.Once);
        }
        
    }
}
