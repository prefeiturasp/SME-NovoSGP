using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase useCase;

        public RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirPendenciaPerfilUsuarioCommand>(), It.IsAny<CancellationToken>()))
               .Returns(Unit.Task);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);
            useCase = new RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase(mediatorMock.Object);
        }

        private MensagemRabbit CriarMensagemRabbitComPerfil(int perfilCodigo, FuncionarioCargoDTO funcionarioAtual = null, bool eraCefai = false, bool eraAdmUe = false)
        {
            var pendenciaFuncionario = new PendenciaPerfilUsuarioDto
            {
                Id = 1,
                PendenciaId = 2,
                PerfilCodigo = perfilCodigo,
                UeId = 10
            };

            var filtro = new FiltroPendenciaPerfilUsuarioCefaiAdmUeDto(funcionarioAtual, eraCefai, eraAdmUe, pendenciaFuncionario);

            var mensagem = new MensagemRabbit
            {
                Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(filtro),
                CodigoCorrelacao = Guid.NewGuid()
            };
            return mensagem;
        }

        [Theory]
        [InlineData((int)PerfilUsuario.CP)]
        [InlineData((int)PerfilUsuario.AD)]
        [InlineData((int)PerfilUsuario.DIRETOR)]
        public async Task Executar_Perfis_CP_AD_DIRETOR_Deve_Chamar_Remover_Tratar_Atribuicao_Se_Funcionario_Nulo_Ou_Perfil_Diferente(int perfilCodigo)
        {
            var funcionarioAtual = new FuncionarioCargoDTO
            {
                CargoId = Cargo.Supervisor // diferente do perfilCodigo
            };

            var mensagem = CriarMensagemRabbitComPerfil(perfilCodigo, funcionarioAtual);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPendenciaPerfilUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd => cmd.Rota == RotasRabbitSgpPendencias.RotaTratarAtribuicaoPendenciaUsuarios), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Perfil_CP_NaoDeve_Chamar_Remocao_Se_Funcionario_Atual_Com_Mesmo_Perfil()
        {
            var perfilCodigo = (int)PerfilUsuario.CP;
            var cargoId = (int)Cargo.CP; 

            var funcionarioAtual = new FuncionarioCargoDTO { CargoId = (Cargo)cargoId };
            var mensagem = CriarMensagemRabbitComPerfil(perfilCodigo, funcionarioAtual);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Perfil_AD_Nao_Deve_Chamar_Remocao_Se_Funcionario_Atual_Com_Mesmo_Perfil()
        {
            var perfilCodigo = (int)PerfilUsuario.AD;
            var cargoId = (int)Cargo.AD;

            var funcionarioAtual = new FuncionarioCargoDTO { CargoId = (Cargo)cargoId };
            var mensagem = CriarMensagemRabbitComPerfil(perfilCodigo, funcionarioAtual);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Perfil_Diretor_Nao_Deve_Chamar_Remocao_Se_Funcionario_Atual_Com_Mesmo_Perfil()
        {
            var perfilCodigo = (int)PerfilUsuario.DIRETOR;
            var cargoId = (int)Cargo.Diretor;

            var funcionarioAtual = new FuncionarioCargoDTO { CargoId = (Cargo)cargoId };
            var mensagem = CriarMensagemRabbitComPerfil(perfilCodigo, funcionarioAtual);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Perfil_Cefai_Deve_Chamar_Remover_Tratar_Atribuicao_Se_Era_Cefai_True()
        {
            var perfilCodigo = (int)PerfilUsuario.CEFAI;
            var mensagem = CriarMensagemRabbitComPerfil(perfilCodigo, null, eraCefai: true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPendenciaPerfilUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Perfil_Cefai_Nao_Deve_Chamar_Remover_Se_Era_Cefai_False()
        {
            var perfilCodigo = (int)PerfilUsuario.CEFAI;
            var mensagem = CriarMensagemRabbitComPerfil(perfilCodigo, null, eraCefai: false);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Perfil_AdmUe_Deve_Chamar_Remover_Tratar_Atribuicao_Se_Era_Adm_Ue_True()
        {
            var perfilCodigo = (int)PerfilUsuario.ADMUE;
            var mensagem = CriarMensagemRabbitComPerfil(perfilCodigo, null, eraAdmUe: true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPendenciaPerfilUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Perfil_Adm_Ue_Nao_Deve_Chamar_Remover_Se_Era_Adm_Ue_False()
        {
            var perfilCodigo = (int)PerfilUsuario.ADMUE;
            var mensagem = CriarMensagemRabbitComPerfil(perfilCodigo, null, eraAdmUe: false);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Negocio_Exception_Se_Exception_For_Arremessada()
        {
            var mensagem = CriarMensagemRabbitComPerfil((int)PerfilUsuario.CP);
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirPendenciaPerfilUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(mensagem));
            Assert.Contains("Erro na remoção de atribuição", ex.Message);
        }
    }
}
