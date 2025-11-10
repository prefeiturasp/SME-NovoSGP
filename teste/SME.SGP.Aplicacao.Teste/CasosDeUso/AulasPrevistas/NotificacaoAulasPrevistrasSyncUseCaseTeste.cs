using FluentValidation.TestHelper;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AulasPrevistas
{
    public class NotificacaoAulasPrevistrasSyncUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificacaoAulasPrevistrasSyncUseCase useCase;

        public NotificacaoAulasPrevistrasSyncUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificacaoAulasPrevistrasSyncUseCase(mediatorMock.Object);
        }

        private RegistroAulaPrevistaDivergenteDto CriarDto() => new RegistroAulaPrevistaDivergenteDto
        {
            Bimestre = 1,
            CodigoDre = "D1",
            CodigoTurma = "T1",
            CodigoUe = "U1",
            DisciplinaId = "MAT",
            NomeDre = "DRE Leste",
            NomeTurma = "Turma A",
            NomeUe = "Escola XPTO",
            ProfessorRf = "12345"
        };

        [Fact]
        public async Task Executar_Deve_Retornar_False_Quando_Quantidade_Dias_Zero()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(string.Empty);

            var mensagem = new MensagemRabbit("acao", "{}", Guid.NewGuid(), "123");

            var result = await useCase.Executar(mensagem);

            Assert.False(result);
        }

        [Fact]
        public async Task Executar_Deve_PublicarMensagens_Quando_Existem_Turmas()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5");

            var turmas = new List<RegistroAulaPrevistaDivergenteDto> { CriarDto() };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasAulasPrevistasDivergentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = new MensagemRabbit("acao", "{}", Guid.NewGuid(), "123");

            var result = await useCase.Executar(mensagem);

            Assert.True(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Obter_Valor_Parametro_Sistema_Tipo_E_Ano_Query_Deve_Preencher_Propriedades()
        {
            var query = new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeDiasNotificarProfessor, 2025);
            Assert.Equal(TipoParametroSistema.QuantidadeDiasNotificarProfessor, query.Tipo);
            Assert.Equal(2025, query.Ano);
        }

        [Fact]
        public void Obter_Parametro_Sistema_Tipo_E_Ano_Query_Validator_Deve_Validar()
        {
            var validator = new ObterParametroSistemaTipoEAnoQueryValidator();

            var queryInvalida = new ObterValorParametroSistemaTipoEAnoQuery(0);
            var resultInvalido = validator.TestValidate(queryInvalida);
            resultInvalido.ShouldHaveValidationErrorFor(q => q.Tipo);

            var queryValida = new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeDiasNotificarProfessor);
            var resultValido = validator.TestValidate(queryValida);
            resultValido.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Obter_Turmas_Aulas_Previstas_Divergentes_Query_Deve_Preencher()
        {
            var query = new ObterTurmasAulasPrevistasDivergentesQuery(10);
            Assert.Equal(10, query.QuantidadeDiasBimestreNotificacao);
        }

        [Fact]
        public void Publicar_Fila_Sgp_Command_Deve_Preencher_Propriedades()
        {
            var usuario = new Usuario();
            var command = new PublicarFilaSgpCommand("rota1", "payload", Guid.NewGuid(), usuario, true, "exchange1");

            Assert.Equal("rota1", command.Rota);
            Assert.Equal("payload", command.Filtros);
            Assert.NotEqual(Guid.Empty, command.CodigoCorrelacao);
            Assert.Equal(usuario, command.Usuario);
            Assert.True(command.NotificarErroUsuario);
            Assert.Equal("exchange1", command.Exchange);
        }

        [Fact]
        public void Publicar_Fila_Sgp_Command_Validator_Deve_Validar()
        {
            var validator = new PublicarFilaSgpCommandValidator();

            var invalido = new PublicarFilaSgpCommand("", null);
            var resultInvalido = validator.TestValidate(invalido);
            resultInvalido.ShouldHaveValidationErrorFor(c => c.Filtros);
            resultInvalido.ShouldHaveValidationErrorFor(c => c.Rota);

            var valido = new PublicarFilaSgpCommand("rota", "filtros");
            var resultValido = validator.TestValidate(valido);
            resultValido.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Mensagem_Rabbit_Deve_Criar_Pelos_Construtores()
        {
            var dto = CriarDto();

            var msg1 = new MensagemRabbit("acao", dto, Guid.NewGuid(), "RF1");
            Assert.Equal("acao", msg1.Action);

            var msg2 = new MensagemRabbit(dto, Guid.NewGuid(), "Nome", "RF2", Guid.NewGuid());
            Assert.Equal("Nome", msg2.UsuarioLogadoNomeCompleto);

            var msg3 = new MensagemRabbit(dto);
            Assert.Equal(dto, msg3.Mensagem);

            var msg4 = new MensagemRabbit();
            Assert.Null(msg4.Mensagem);

            var serializado = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
            var msg5 = new MensagemRabbit(serializado);
            var obj = msg5.ObterObjetoMensagem<RegistroAulaPrevistaDivergenteDto>();
            Assert.Equal(dto.ProfessorRf, obj.ProfessorRf);
        }
    }
}
