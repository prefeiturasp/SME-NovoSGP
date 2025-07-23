using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class TratarAtribuicaoPendenciasUsuariosUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly TratarAtribuicaoPendenciasUsuariosUseCase useCase;

        public TratarAtribuicaoPendenciasUsuariosUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new TratarAtribuicaoPendenciasUsuariosUseCase(mediatorMock.Object);
        }

        private MensagemRabbit CriarMensagem(long pendenciaId, long ueId)
        {
            var dto = new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaId, ueId);
            return new MensagemRabbit(JsonConvert.SerializeObject(dto));
        }

        [Theory]
        [InlineData(PerfilUsuario.CP)]
        [InlineData(PerfilUsuario.AD)]
        [InlineData(PerfilUsuario.DIRETOR)]
        public async Task Executar_Deve_Tratar_Perfis_De_Gestao(PerfilUsuario perfil)
        {
            var pendenciaId = 1L;
            var ueId = 1234L;
            var pendenciaPerfilId = 99L;
            var mensagem = CriarMensagem(pendenciaId, ueId);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciaPerfilPorPendenciaIdQuery>(), default))
                .ReturnsAsync(new List<PendenciaPerfil>
                {
                new PendenciaPerfil
                {
                    Id = pendenciaPerfilId,
                    PerfilCodigo = perfil,
                    PendenciasPerfilUsuarios = new List<PendenciaPerfilUsuario>() 
                }
                });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), default))
                .ReturnsAsync(new DreUeCodigoDto { DreCodigo = "DRE1", UeCodigo = "UE1" });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterFuncionariosPorCargoHierarquicoQuery>(), default))
                .ReturnsAsync(new List<FuncionarioCargoDTO>());

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterFuncionariosPorFuncaoExternaHierarquicoQuery>(), default))
                .ReturnsAsync(new List<FuncionarioFuncaoExternaDTO>());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Tratar_Perfil_CEFAI()
        {
            var pendenciaId = 1L;
            var ueId = 1234L;
            var pendenciaPerfilId = 99L;
            var mensagem = CriarMensagem(pendenciaId, ueId);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciaPerfilPorPendenciaIdQuery>(), default))
                .ReturnsAsync(new List<PendenciaPerfil>
                {
                new PendenciaPerfil
                {
                    Id = pendenciaPerfilId,
                    PerfilCodigo = PerfilUsuario.CEFAI,
                    PendenciasPerfilUsuarios = new List<PendenciaPerfilUsuario>()
                }
                });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), default))
                .ReturnsAsync(new DreUeCodigoDto { DreCodigo = "DRE01", UeCodigo = "UE01" });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObtemUsuarioCEFAIDaDreQuery>(), default))
                .ReturnsAsync(new List<long> { 77L });

            mediatorMock.Setup(x => x.Send(It.IsAny<VerificaExistenciaDePendenciaPerfilUsuarioQuery>(), default))
                .ReturnsAsync(false);

            mediatorMock.Setup(x => x.Send(It.IsAny<SalvarPendenciaPerfilUsuarioCommand>(), default))
                .Returns(Unit.Task);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Tratar_Perfil_ADM_UE()
        {
            var pendenciaId = 1L;
            var ueId = 1234L;
            var pendenciaPerfilId = 88L;
            var mensagem = CriarMensagem(pendenciaId, ueId);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciaPerfilPorPendenciaIdQuery>(), default))
                .ReturnsAsync(new List<PendenciaPerfil>
                {
                new PendenciaPerfil
                {
                    Id = pendenciaPerfilId,
                    PerfilCodigo = PerfilUsuario.ADMUE,
                    PendenciasPerfilUsuarios = new List<PendenciaPerfilUsuario>()
                }
                });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), default))
                .ReturnsAsync(new DreUeCodigoDto { DreCodigo = "DRE01", UeCodigo = "UE01" });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterAdministradoresPorUEQuery>(), default))
                .ReturnsAsync(new[] { "1234567" });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), default))
                .ReturnsAsync(100L);

            mediatorMock.Setup(x => x.Send(It.IsAny<VerificaExistenciaDePendenciaPerfilUsuarioQuery>(), default))
                .ReturnsAsync(false);

            mediatorMock.Setup(x => x.Send(It.IsAny<SalvarPendenciaPerfilUsuarioCommand>(), default))
                .Returns(Unit.Task);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Theory]
        [InlineData(PerfilUsuario.ADMSME)]
        [InlineData(PerfilUsuario.SECRETARIO)]
        [InlineData(PerfilUsuario.SUPERVISOR)]
        public async Task Executar_Deve_Ignorar_Perfis_Sem_Tratamento(PerfilUsuario perfil)
        {
            var pendenciaId = 1;
            var mensagem = CriarMensagem(pendenciaId, 1234);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciaPerfilPorPendenciaIdQuery>(), default))
                .ReturnsAsync(new List<PendenciaPerfil>
                {
                new PendenciaPerfil
                {
                    Id = 10,
                    PerfilCodigo = perfil,
                    PendenciasPerfilUsuarios = new List<PendenciaPerfilUsuario>()
                }
                });

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<SalvarPendenciaPerfilUsuarioCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_Nao_Deve_Atribuir_Se_Pendencia_Perfil_Ja_Possui_Usuario()
        {
            var pendenciaId = 1L;
            var ueId = 1234L;
            var mensagem = CriarMensagem(pendenciaId, ueId);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciaPerfilPorPendenciaIdQuery>(), default))
                .ReturnsAsync(new List<PendenciaPerfil>
                {
            new PendenciaPerfil
                {
                    Id = 50,
                    PerfilCodigo = PerfilUsuario.CP,
                    PendenciasPerfilUsuarios = new List<PendenciaPerfilUsuario>
                    {
                        new PendenciaPerfilUsuario()
                    }
                }
            });

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<SalvarPendenciaPerfilUsuarioCommand>(), default), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Se_Mensagem_Invalida()
        {
            var mensagem = new MensagemRabbit { Mensagem = "string_invalida_json" };

            await Assert.ThrowsAnyAsync<Newtonsoft.Json.JsonReaderException>(() =>
                useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Se_Mediator_Falhar()
        {
            var pendenciaId = 1L;
            var ueId = 1234L;
            var mensagem = CriarMensagem(pendenciaId, ueId);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPendenciaPerfilPorPendenciaIdQuery>(), default))
                .ThrowsAsync(new InvalidOperationException("Erro interno"));

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await useCase.Executar(mensagem));

            Assert.Equal("Erro interno", exception.Message);
        }

    }
}
