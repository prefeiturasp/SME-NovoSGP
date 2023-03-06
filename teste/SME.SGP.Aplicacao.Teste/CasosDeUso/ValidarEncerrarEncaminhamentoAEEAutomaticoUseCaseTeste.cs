using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ValidarEncerrarEncaminhamentoAEEAutomaticoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ValidarEncerrarEncaminhamentoAEEAutomaticoUseCase _validarEncerrarEncaminhamentoAEEAutomaticoUseCase;

        public ValidarEncerrarEncaminhamentoAEEAutomaticoUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _validarEncerrarEncaminhamentoAEEAutomaticoUseCase = new ValidarEncerrarEncaminhamentoAEEAutomaticoUseCase(_mediator.Object);
        }

        [Fact]
        public async Task Deve_Validar_Situacao_Inativa_Encaminhamento_AEE()
        {
            //-> Arrange
            var alunoPorUe = new AlunoPorUeDto
            {
                CodigoAluno = "123",
                NomeAluno = "ALUNO TESTE",
                NomeSocialAluno = string.Empty,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                SituacaoMatricula = "Transferido",
                DataSituacao = DateTime.UtcNow.AddDays(-1),
                CodigoTurma = 1,
                CodigoMatricula = 1,
                AnoLetivo = 2022
            };

            var matriculasTurmaAlunoEol = new List<AlunoPorUeDto>
            {
                alunoPorUe
            };

            _mediator.Setup(c => c.Send(It.IsAny<ObterMatriculasAlunoNaUEQuery>(), new CancellationToken()))
                .ReturnsAsync(matriculasTurmaAlunoEol);

            //-> Act
            var retorno = await _validarEncerrarEncaminhamentoAEEAutomaticoUseCase.Executar(CriarMensagemRabbitValidarEncerrarEncaminhamentoAEEAutomatico());

            //-> Assert
            Assert.True(retorno);
        }

        private MensagemRabbit CriarMensagemRabbitValidarEncerrarEncaminhamentoAEEAutomatico()
        {
            var filtro = new FiltroValidarEncerrarEncaminhamentoAEEAutomaticoDto(1983, "4824410", "094668", 2022);
            var json = JsonConvert.SerializeObject(filtro);

            return new MensagemRabbit(json)
            { 
                Action = "sgp/encaminhamento/aee/encerrar/automatico/validar",
                UsuarioLogadoNomeCompleto = "TESTE",
                UsuarioLogadoRF = "999"
            };
        }
    }
}
