using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Threading;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.Commands.Frequencia.InserirFrequenciasAula
{
    public class InserirFrequenciasAulaCommandHandlerTeste
    {
        private const string LOGIN_SISTEMA = "Sistema";
        private readonly Mock<IMediator> mediator;
        private readonly InserirFrequenciasAulaCommandHandler command;

        public InserirFrequenciasAulaCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            command = new InserirFrequenciasAulaCommandHandler(mediator.Object);
        }

        [Fact(DisplayName = "InserirFrequenciasAula - Deve inserir frequencias aula considerando usuário do sistema")]
        public async Task DeveInserirFrequenciasAulaConsiderandoUsuarioSistema()
        {
            var dataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 2, 15);

            mediator.Setup(x => x.Send(It.Is<ObterUsuarioPorCodigoRfLoginQuery>(y => y.Login == LOGIN_SISTEMA), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario() { Login = LOGIN_SISTEMA });

            mediator.Setup(x => x.Send(It.Is<ObterAulaPorIdQuery>(y => y.AulaId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Aula() { Id = 1, TurmaId = "1", DisciplinaId = "1", DataAula = dataAula });

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { Id = 1, CodigoTurma = "1" });

            mediator.Setup(x => x.Send(It.Is<VerificaPodePersistirTurmaDisciplinaQuery>(y => y.Usuario.Login == LOGIN_SISTEMA && y.TurmaId == "1" && y.ComponenteCurricularId == "1" && y.Data.Date == dataAula.Date && !y.Historico), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.Is<ObterBimestreAtualQuery>(y => y.DataReferencia == dataAula && y.Turma.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.Is<TurmaEmPeriodoAbertoQuery>(y => y.Turma.CodigoTurma == "1" && y.DataReferencia.Date == DateTimeExtension.HorarioBrasilia().Date && y.Bimestre == 1 && y.EhAnoLetivo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.Is<PersistirRegistroFrequenciaCommand>(y => y.RegistroFrequencia.AulaId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.Is<InserirRegistrosFrequenciasAlunosCommand>(y => y.RegistroFrequenciaId == 1 && y.TurmaId == 1 && y.ComponenteCurricularId == 1 && y.AulaId == 1 && y.DataAula == dataAula), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var requestCommand = new InserirFrequenciasAulaCommand(new FrequenciaDto()
            {
                ListaFrequencia = new List<RegistroFrequenciaAlunoDto>() { new() { CodigoAluno = "1" } },
                AulaId = 1
            }, usuarioLogin: "Sistema");

            var resultado = await command.Handle(requestCommand, It.IsAny<CancellationToken>());

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Auditoria.Id);
            Assert.Equal(DateTimeExtension.HorarioBrasilia().Date, resultado.Auditoria.CriadoEm.Date);
            Assert.Equal("1", resultado.TurmaId);
            Assert.Equal("1", resultado.DisciplinaId);
        }
    }
}
