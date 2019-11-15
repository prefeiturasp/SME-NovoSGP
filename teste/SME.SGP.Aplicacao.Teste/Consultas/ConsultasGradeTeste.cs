using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasGradeTeste
    {
        private readonly ConsultasGrade consultasGrade;
        private readonly Mock<IRepositorioGrade> repositorioGrade;
        private readonly Mock<IServicoUsuario> servicoUsuario;

        public ConsultasGradeTeste()
        {
            repositorioGrade = new Mock<IRepositorioGrade>();
            servicoUsuario = new Mock<IServicoUsuario>();

            consultasGrade = new ConsultasGrade(repositorioGrade.Object, servicoUsuario.Object);

            Setup();
        }

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

        [Fact]
        public async Task DeveObterGradeTurma()
        {

            var gradeDto = await consultasGrade.ObterGradeTurma(TipoEscola.EMEBS, Modalidade.Fundamental, 5);

            Assert.NotNull(gradeDto);
            Assert.True(gradeDto.Id == 1);
        }

        [Fact]
        public async Task DeveObterHorasGradeComponente()
        {
            var horasGrade = await consultasGrade.ObterHorasGradeComponente(1, 7, 4);

            Assert.True(horasGrade == 5);
        }
    }
}
