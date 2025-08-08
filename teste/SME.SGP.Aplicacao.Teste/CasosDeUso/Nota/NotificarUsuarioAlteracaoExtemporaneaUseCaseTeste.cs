using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Nota
{
    public class NotificarUsuarioAlteracaoExtemporaneaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IServicoUsuario> servicoUsuarioMock;
        private readonly NotificarUsuarioAlteracaoExtemporaneaUseCase useCase;

        public NotificarUsuarioAlteracaoExtemporaneaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            servicoUsuarioMock = new Mock<IServicoUsuario>();
            useCase = new NotificarUsuarioAlteracaoExtemporaneaUseCase(mediatorMock.Object, servicoUsuarioMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Notificar_Usuarios_CP_E_Um_Diretor()
        {
            var atividade = new AtividadeAvaliativa
            {
                DreId = "dre-1",
                UeId = "ue-1",
                TurmaId = "turma-1"
            };

            var filtro = new FiltroNotificarUsuarioAlteracaoExtemporaneaDto(atividade, "Mensagem", "Turma A", "123456");

            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            var funcionarios = new List<FuncionarioDTO>
            {
                new FuncionarioDTO { CodigoRF = "123" },
                new FuncionarioDTO { CodigoRF = "456" }
            };

            var usuarios = new List<Usuario>
            {
                new Usuario { CodigoRf = "123", Id = 1 },
                new Usuario { CodigoRf = "456", Id = 2 }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterFuncionariosPorUeECargoQuery>(q =>
                    q.CodigoUE == "123456" && q.CodigoCargo == (int)Cargo.CP), It.IsAny<CancellationToken>()))
                .ReturnsAsync(funcionarios);

            servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona("123", "", "", "", false))
                .ReturnsAsync(usuarios[0]);

            servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona("456", "", "", "", false))
                .ReturnsAsync(usuarios[1]);

            var comandosEnviados = new List<NotificarUsuarioCommand>();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<long>, CancellationToken>((cmd, _) =>
                {
                    comandosEnviados.Add((NotificarUsuarioCommand)cmd);
                })
                .ReturnsAsync(1);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            comandosEnviados.Should().HaveCount(3); 
            comandosEnviados.Should().OnlyContain(cmd =>
                cmd.Titulo.Contains("Turma A") &&
                cmd.Mensagem == "Mensagem" &&
                cmd.Categoria == NotificacaoCategoria.Alerta &&
                cmd.Tipo == NotificacaoTipo.Notas &&
                cmd.DreCodigo == "dre-1" &&
                cmd.UeCodigo == "ue-1" &&
                cmd.TurmaCodigo == "turma-1");
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Mas_Nao_Notificar_Se_Nao_Houver_Funcionarios_CP()
        {
            var filtro = new FiltroNotificarUsuarioAlteracaoExtemporaneaDto(
                new AtividadeAvaliativa { DreId = "dre", UeId = "ue", TurmaId = "turma" },
                "msg", "Turma X", "999");

            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFuncionariosPorUeECargoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FuncionarioDTO>());

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue(); 

            mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Notificar_Duas_Vezes_Quando_Diretor_E_For_Iterado()
        {
            var atividade = new AtividadeAvaliativa { DreId = "dre", UeId = "ue", TurmaId = "turma" };
            var filtro = new FiltroNotificarUsuarioAlteracaoExtemporaneaDto(atividade, "Texto", "Turma Z", "111");
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            var funcionarios = new List<FuncionarioDTO>
            {
                new FuncionarioDTO { CodigoRF = "321" }
            };

            var usuario = new Usuario { CodigoRf = "321", Id = 123 };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFuncionariosPorUeECargoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(funcionarios);

            servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona("321", "", "", "", false))
                .ReturnsAsync(usuario);

            var comandos = new List<NotificarUsuarioCommand>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<long>, CancellationToken>((cmd, _) => comandos.Add((NotificarUsuarioCommand)cmd))
                .ReturnsAsync(1);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            comandos.Count.Should().Be(2); 
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Se_Dependencias_Forem_Nulas()
        {
            Action act1 = () => new NotificarUsuarioAlteracaoExtemporaneaUseCase(null, servicoUsuarioMock.Object);
            Action act2 = () => new NotificarUsuarioAlteracaoExtemporaneaUseCase(mediatorMock.Object, null);

            act1.Should().Throw<ArgumentNullException>();
            act2.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Executar_Deve_Ignorar_Se_Usuario_Nulo()
        {
            var atividade = new AtividadeAvaliativa { DreId = "dre", UeId = "ue", TurmaId = "turma" };
            var filtro = new FiltroNotificarUsuarioAlteracaoExtemporaneaDto(atividade, "Texto", "Turma Y", "222");

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFuncionariosPorUeECargoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FuncionarioDTO> { new FuncionarioDTO { CodigoRF = "999" } });

            servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona("999", "", "", "", false))
                .ReturnsAsync((Usuario)null); 

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Ignorar_Usuarios_Nulos_Durante_Iteracao()
        {
            var atividade = new AtividadeAvaliativa { DreId = "dre", UeId = "ue", TurmaId = "turma" };
            var filtro = new FiltroNotificarUsuarioAlteracaoExtemporaneaDto(atividade, "Texto", "Turma X", "111");
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync((string codigoRf, string _, string __, string ___, bool ____) =>
                    codigoRf switch
                    {
                        "999" => new Usuario { Id = 0, CodigoRf = "999" },
                        "123" => new Usuario { Id = 123456, CodigoRf = "123" },
                        _ => null
                    });

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFuncionariosPorUeECargoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FuncionarioDTO>
                {
                   new FuncionarioDTO { CodigoRF = "999" },
                   new FuncionarioDTO { CodigoRF = "123" }
                });

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(3)); // 2 do foreach + 1 do diretor

            mediatorMock.Verify(m => m.Send(It.Is<NotificarUsuarioCommand>(c => c.UsuarioRf == "123"), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task Executar_Deve_Ignorar_Notificacao_Se_UsuarioDiretor_For_Nulo()
        {
            var atividade = new AtividadeAvaliativa { DreId = "dre", UeId = "ue", TurmaId = "turma" };
            var filtro = new FiltroNotificarUsuarioAlteracaoExtemporaneaDto(atividade, "Texto", "Turma Z", "333");
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            mediatorMock
                .SetupSequence(m => m.Send(It.IsAny<ObterFuncionariosPorUeECargoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FuncionarioDTO>
                {
                    new FuncionarioDTO { CodigoRF = "123" }
                })
                .ReturnsAsync(new List<FuncionarioDTO>
                {
                    new FuncionarioDTO { CodigoRF = "999" } 
                });

            servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona("123", "", "", "", false))
                .ReturnsAsync(new Usuario { Id = 1234567, CodigoRf = "123" });

            servicoUsuarioMock
                .Setup(s => s.ObterUsuarioPorCodigoRfLoginOuAdiciona("999", "", "", "", false))
                .ReturnsAsync((Usuario)null);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();

            mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
