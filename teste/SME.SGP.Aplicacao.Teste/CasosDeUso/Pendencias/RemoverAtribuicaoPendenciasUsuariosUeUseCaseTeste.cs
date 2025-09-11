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

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class RemoverAtribuicaoPendenciasUsuariosUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly RemoverAtribuicaoPendenciasUsuariosUeUseCase _useCase;

        public RemoverAtribuicaoPendenciasUsuariosUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new RemoverAtribuicaoPendenciasUsuariosUeUseCase(_mediatorMock.Object);
        }

        private MensagemRabbit CriarMensagem(PendenciaPerfilUsuarioDto dto)
        {
            var filtro = new FiltroRemoverAtribuicaoPendenciaDto(1, new List<PendenciaPerfilUsuarioDto> { dto });
            var json = JsonConvert.SerializeObject(filtro); // CORREÇÃO: serializa o objeto
            return new MensagemRabbit("rota.teste", json, Guid.NewGuid(), "1234567");
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Perfis_Comuns_Forem_Processados()
        {
            var rf = "1234567";
            var pendencia = new PendenciaPerfilUsuarioDto
            {
                PendenciaId = 100,
                PerfilCodigo = (int)PerfilUsuario.CP,
                CodigoRf = rf,
                UeId = 99
            };

            var funcionarios = new List<FuncionarioCargoDTO>
            {
                new FuncionarioCargoDTO(rf, Cargo.CP)
            };

            var pendenciaPerfis = new List<PendenciaPerfil>
            {
                new PendenciaPerfil { Id = 555, PerfilCodigo = PerfilUsuario.CP, PendenciaId = 100 }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DreUeCodigoDto { UeCodigo = "99", DreCodigo = "01" });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFuncionariosPorCargoHierarquicoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(funcionarios);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPendenciaPerfilPorPendenciaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendenciaPerfis);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(999);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarPendenciaPerfilUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Unit.Task);

            _mediatorMock
                .Setup(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                    c.Rota == RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUeFuncionario), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = CriarMensagem(pendencia);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Negocio_Exception_Se_Ocorrer_Erro()
        {
            var pendencia = new PendenciaPerfilUsuarioDto
            {
                PendenciaId = 100,
                PerfilCodigo = (int)PerfilUsuario.CP,
                CodigoRf = "123",
                UeId = 99
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro"));

            var mensagem = CriarMensagem(pendencia);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagem));
            Assert.Equal("Erro na remoção de atribuição de Pendência Perfil Usuário por UE.", ex.Message);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comando_Para_Cefai_Quando_Usuario_Id_Nao_Esta_Na_Lista()
        {
            var pendencia = new PendenciaPerfilUsuarioDto
            {
                PendenciaId = 101,
                PerfilCodigo = (int)PerfilUsuario.CEFAI,
                UsuarioId = 999,
                UeId = 88
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DreUeCodigoDto { UeCodigo = "88", DreCodigo = "01" });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObtemUsuarioCEFAIDaDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<long> { 111, 222 }); 

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = CriarMensagem(pendencia);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Filtros is FiltroPendenciaPerfilUsuarioCefaiAdmUeDto), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comando_Para_Adm_Ue_Quando_Usuario_Id_Nao_Esta_Na_Lista()
        {
            var pendencia = new PendenciaPerfilUsuarioDto
            {
                PendenciaId = 102,
                PerfilCodigo = (int)PerfilUsuario.ADMUE,
                UsuarioId = 555,
                UeId = 77
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DreUeCodigoDto { UeCodigo = "77", DreCodigo = "01" });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAdministradoresPorUEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { "777777" }); 

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(999); 

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = CriarMensagem(pendencia);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Filtros is FiltroPendenciaPerfilUsuarioCefaiAdmUeDto), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Criar_Pendencia_Perfil_Usuario_Se_Funcionario_Nao_Possui()
        {
            var rfFuncionarioNovo1 = "8888888"; 
            var rfFuncionarioNovo2 = "7777777"; 
            var rfComPendencia = "9999999";     

            var pendencia = new PendenciaPerfilUsuarioDto
            {
                PendenciaId = 200,
                PerfilCodigo = (int)PerfilUsuario.DIRETOR,
                CodigoRf = rfComPendencia,
                UeId = 12
            };

            var funcionarios = new List<FuncionarioCargoDTO>
            {
                new FuncionarioCargoDTO { FuncionarioRF = rfFuncionarioNovo1 },
                new FuncionarioCargoDTO { FuncionarioRF = rfFuncionarioNovo2 }
            };

            var pendenciaPerfis = new List<PendenciaPerfil>
            {
                new PendenciaPerfil { Id = 1000, PerfilCodigo = PerfilUsuario.DIRETOR }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigoUEDREPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DreUeCodigoDto { UeCodigo = "12", DreCodigo = "01" });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFuncionariosPorCargoHierarquicoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(funcionarios);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPendenciaPerfilPorPendenciaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendenciaPerfis);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(5000);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarPendenciaPerfilUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Unit.Task);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = CriarMensagem(pendencia);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPendenciaPerfilUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}
