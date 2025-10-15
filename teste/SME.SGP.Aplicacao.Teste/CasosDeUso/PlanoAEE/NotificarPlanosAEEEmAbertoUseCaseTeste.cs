using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class NotificarPlanosAEEEmAbertoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly NotificarPlanosAEEEmAbertoUseCase useCase;

        public NotificarPlanosAEEEmAbertoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new NotificarPlanosAEEEmAbertoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Quando_Parametro_Notificacao_Inativo()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Quando_Datas_Parametro_Nao_Encontradas()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ParametrosSistema { Ativo = true });

            mediator.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<ParametrosSistema>)null);

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Quando_Parametro_Existe_Mas_Inativo()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = false }); // Parâmetro inativo

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.False(resultado);
            mediator.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Executar_Fluxo_Completo_Quando_Todos_Parametros_Validos()
        {
            // Arrange
            var hoje = DateTime.Today;
            var planos = new List<PlanoAEEReduzidoDto>
        {
        new PlanoAEEReduzidoDto
        {
            UECodigo = "1",
            DREAbreviacao = "DRE1",
            UETipo = Dominio.TipoEscola.EMEF,
            UENome = "Escola Teste",
            EstudanteNome = "Aluno Teste",
            EstudanteCodigo = "123",
            TurmaModalidade = Modalidade.Fundamental,
            TurmaNome = "Turma A",
            VigenciaInicio = hoje.AddDays(-10),
            VigenciaFim = hoje.AddDays(10),
            Situacao = SituacaoPlanoAEE.ParecerCP
        }
        };
            var supervisores = new List<FuncionarioDTO> { new FuncionarioDTO { CodigoRF = "123456" } };

            // Setup dos mocks
            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true, Valor = "S" });

            mediator.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ParametrosSistema> {
            new ParametrosSistema {
                Valor = hoje.ToString("dd/MM"), // Formato dia/mês
                Ativo = true
            }
                });

            mediator.Setup(x => x.Send(ObterPlanosAEEAtivosComTurmaEVigenciaQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(planos);

            mediator.Setup(x => x.Send(It.Is<ObterFuncionariosPorUeECargoQuery>(q => q.CodigoCargo == (int)Cargo.Supervisor), It.IsAny<CancellationToken>()))
                .ReturnsAsync(supervisores);

            mediator.Setup(x => x.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()));

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.True(resultado);

            // Verificações específicas
            mediator.Verify(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(ObterPlanosAEEAtivosComTurmaEVigenciaQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.Is<ObterFuncionariosPorUeECargoQuery>(q => q.CodigoCargo == (int)Cargo.Supervisor), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Parametro_Sistema_Inativo()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = false });

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.False(resultado);

            // Verifica que não foram feitas outras consultas
            mediator.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(x => x.Send(ObterPlanosAEEAtivosComTurmaEVigenciaQuery.Instance, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Deve_Usar_Supervisor_Tecnico_Quando_Supervisor_Nao_Encontrado()
        {
            // Arrange
            var hoje = DateTime.Today;
            var planos = new List<PlanoAEEReduzidoDto>
    {
        new PlanoAEEReduzidoDto
        {
            UECodigo = "1",
            DREAbreviacao = "DRE1",
            UETipo = Dominio.TipoEscola.EMEF,
            UENome = "Escola Teste",
            EstudanteNome = "Aluno Teste",
            EstudanteCodigo = "123",
            TurmaModalidade = Modalidade.Fundamental,
            TurmaNome = "Turma A",
            VigenciaInicio = hoje.AddDays(-10),
            VigenciaFim = hoje.AddDays(10),
            Situacao = SituacaoPlanoAEE.ParecerCP
        }
    };
            var supervisoresTecnicos = new List<FuncionarioDTO> { new FuncionarioDTO { CodigoRF = "654321" } };

            // Setup correto dos mocks
            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true, Valor = "S" });

            mediator.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ParametrosSistema> {
            new ParametrosSistema {
                Valor = hoje.ToString("dd/MM"), // Formato correto: dia/mês
                Ativo = true
            }
                });

            mediator.Setup(x => x.Send(ObterPlanosAEEAtivosComTurmaEVigenciaQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(planos);

            // Setup para não encontrar supervisor comum
            mediator.Setup(x => x.Send(It.Is<ObterFuncionariosPorUeECargoQuery>(q => q.CodigoCargo == (int)Cargo.Supervisor), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FuncionarioDTO>());

            // Setup para encontrar supervisor técnico
            mediator.Setup(x => x.Send(It.Is<ObterFuncionariosPorUeECargoQuery>(q => q.CodigoCargo == (int)Cargo.SupervisorTecnico), It.IsAny<CancellationToken>()))
                .ReturnsAsync(supervisoresTecnicos);

            mediator.Setup(x => x.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()));

            // Act
            var resultado = await useCase.Executar(new MensagemRabbit());

            // Assert
            Assert.True(resultado);

            // Verifica que tentou buscar supervisor comum primeiro
            mediator.Verify(x => x.Send(It.Is<ObterFuncionariosPorUeECargoQuery>(q => q.CodigoCargo == (int)Cargo.Supervisor), It.IsAny<CancellationToken>()), Times.Once);

            // Verifica que buscou supervisor técnico como fallback
            mediator.Verify(x => x.Send(It.Is<ObterFuncionariosPorUeECargoQuery>(q => q.CodigoCargo == (int)Cargo.SupervisorTecnico), It.IsAny<CancellationToken>()), Times.Once);

            // Verifica que notificou o supervisor técnico
            mediator.Verify(x => x.Send(It.Is<NotificarUsuarioCommand>(c => c.UsuarioRf == "654321"), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
