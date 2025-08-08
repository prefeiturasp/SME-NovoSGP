using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtividadeAvaliativa
{
    public class ImportarAtividadeGsaCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ImportarAtividadeGsaCommandHandler command;

        public ImportarAtividadeGsaCommandHandlerTeste()
        {
            mediatorMock = new Mock<IMediator>();
            command = new ImportarAtividadeGsaCommandHandler(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Reagendar_Importacao_Se_Aula_For_Nula()
        {
            var dataCriacao = DateTime.Now;
            var atividadeGsa = new AtividadeGsaDto
            {
                TurmaId = "123",
                ComponenteCurricularId = 456,
                DataCriacao = dataCriacao
            };

            var parametroSistemaDto = new ParametrosSistema
            {
                Valor = dataCriacao.AddDays(-1).ToString("yyyy-MM-dd")
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroSistemaDto);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAulaPorCodigoTurmaComponenteEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DataAulaDto)null);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            var command1 = new ImportarAtividadeGsaCommand(atividadeGsa);

            await ((IRequestHandler<ImportarAtividadeGsaCommand, Unit>)command).Handle(command1, CancellationToken.None);

            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Salvar_Atividade_Infantil_Se_Aula_For_Infantil()
        {
            var dataCriacao = DateTime.Today;
            var atividadeGsa = new AtividadeGsaDto
            {
                TurmaId = "123",
                ComponenteCurricularId = 456,
                DataCriacao = dataCriacao
            };

            var aula = new DataAulaDto { AulaId = 10, DataAula = dataCriacao, EhModalidadeInfantil = true };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = dataCriacao.AddDays(-1).ToString("yyyy-MM-dd") });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterAulaPorCodigoTurmaComponenteEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);

            mediatorMock.Setup(x => x.Send(It.IsAny<SalvarAtividadeInfantilCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            await ((IRequestHandler<ImportarAtividadeGsaCommand, Unit>)command).Handle(new ImportarAtividadeGsaCommand(atividadeGsa), CancellationToken.None);

            mediatorMock.Verify(x => x.Send(It.IsAny<SalvarAtividadeInfantilCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Salvar_Atividade_Avaliativa_Se_Componente_Lanca_Nota()
        {
            var dataCriacao = DateTime.Today;
            var atividadeGsa = new AtividadeGsaDto
            {
                TurmaId = "123",
                ComponenteCurricularId = 456,
                DataCriacao = dataCriacao
            };

            var aula = new DataAulaDto { AulaId = 10, DataAula = dataCriacao, EhModalidadeInfantil = false };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = dataCriacao.AddDays(-1).ToString("yyyy-MM-dd") });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterAulaPorCodigoTurmaComponenteEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(x => x.Send(It.IsAny<SalvarAtividadeAvaliativaGsaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            await ((IRequestHandler<ImportarAtividadeGsaCommand, Unit>)command).Handle(new ImportarAtividadeGsaCommand(atividadeGsa), CancellationToken.None);

            mediatorMock.Verify(x => x.Send(It.IsAny<SalvarAtividadeAvaliativaGsaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Componente_Nao_Lancar_Nota()
        {
            var dataCriacao = DateTime.Today;
            var atividadeGsa = new AtividadeGsaDto
            {
                TurmaId = "123",
                ComponenteCurricularId = 999,
                DataCriacao = dataCriacao
            };

            var aula = new DataAulaDto { AulaId = 10, DataAula = dataCriacao, EhModalidadeInfantil = false };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = dataCriacao.AddDays(-1).ToString("yyyy-MM-dd") });

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterAulaPorCodigoTurmaComponenteEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<NegocioException>(() =>
                ((IRequestHandler<ImportarAtividadeGsaCommand, Unit>)command).Handle(new ImportarAtividadeGsaCommand(atividadeGsa), CancellationToken.None));

            Assert.Contains("Componentes que não lançam nota", ex.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Atividade_For_Ano_Anterior()
        {
            var atividadeGsa = new AtividadeGsaDto
            {
                TurmaId = "123",
                ComponenteCurricularId = 999,
                DataCriacao = new DateTime(DateTime.Today.Year - 1, 12, 31)
            };
          
            var ex = await Assert.ThrowsAsync<NegocioException>(() =>
                ((IRequestHandler<ImportarAtividadeGsaCommand, Unit>)command).Handle(new ImportarAtividadeGsaCommand(atividadeGsa), CancellationToken.None));

            Assert.Contains("Atividade Avaliativa do Classroom de ano anterior", ex.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Atividade_For_Antes_Do_Parametro()
        {
            var dataCriacao = DateTime.Today;
            var atividadeGsa = new AtividadeGsaDto
            {
                TurmaId = "123",
                ComponenteCurricularId = 45,
                DataCriacao = dataCriacao.AddDays(-10)
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Valor = dataCriacao.ToString("yyyy-MM-dd") });

            var ex = await Assert.ThrowsAsync<NegocioException>(() =>
                ((IRequestHandler<ImportarAtividadeGsaCommand, Unit>)command).Handle(new ImportarAtividadeGsaCommand(atividadeGsa), CancellationToken.None));

            Assert.Contains("data anterior ao parâmetro", ex.Message);
        }
    }
}
