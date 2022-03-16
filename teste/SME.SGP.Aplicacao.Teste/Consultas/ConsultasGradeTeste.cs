using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasGradeTeste
    {
        private readonly Mock<IConsultasAula> consultasAula;
        private readonly ConsultasGrade consultasGrade;
        private readonly Mock<IRepositorioGrade> repositorioGrade;
        private readonly Mock<IRepositorioTurmaConsulta> repositorioTurma;
        private readonly Mock<IRepositorioUeConsulta> repositorioUe;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IMediator> mediator;

        public ConsultasGradeTeste()
        {
            repositorioGrade = new Mock<IRepositorioGrade>();
            consultasAula = new Mock<IConsultasAula>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioTurma = new Mock<IRepositorioTurmaConsulta>();
            mediator = new Mock<IMediator>();
            consultasGrade = new ConsultasGrade(repositorioGrade.Object, consultasAula.Object, servicoUsuario.Object, repositorioUe.Object, repositorioTurma.Object, mediator.Object);
            repositorioUe = new Mock<IRepositorioUeConsulta>();

            Setup();
        }

        [Fact]
        public async Task DeveObterGradeTurma()
        {
            var gradeDto = await consultasGrade.ObterGradeTurma(TipoEscola.EMEBS, Modalidade.Fundamental, 5);

            Assert.NotNull(gradeDto);
            Assert.True(gradeDto.Id == 1);
        }

        //[Fact]
        //public async Task DeveObterHorasGradeComponente()
        //{
        //    var horasGrade = await consultasGrade.ObterHorasGradeComponente(1, 7, 4);

        //    Assert.True(horasGrade == 5);
        //}

        //[Fact]
        //public async Task DeveRetornarQuatroAulasGradeParaSRMEAEE()
        //{
        //    var semana = (DateTimeExtension.HorarioBrasilia().DayOfYear / 7) + 1;
        //    var aulasGrade = await consultasGrade.ObterGradeAulasTurmaProfessor("123", 1030, semana, DateTimeExtension.HorarioBrasilia());

        //    Assert.True(aulasGrade.QuantidadeAulasGrade == 4);
        //}

        private void Setup()
        {
            var grade = new Grade()
            {
                Id = 1,
                Nome = ""
            };

            repositorioGrade.Setup(c => c.ObterGradeTurma(It.IsAny<TipoEscola>(), It.IsAny<Modalidade>(), It.IsAny<int>()))
                .Returns(Task.FromResult(grade));

            repositorioGrade.Setup(c => c.ObterHorasComponente(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(5));
        }
    }
}