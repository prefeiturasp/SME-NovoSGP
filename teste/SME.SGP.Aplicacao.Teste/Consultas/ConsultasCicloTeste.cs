using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasCicloTeste
    {
        private readonly ConsultasCiclo consultasCiclo;
        private readonly Mock<IRepositorioCiclo> repositorioCiclo;

        public ConsultasCicloTeste()
        {
            repositorioCiclo = new Mock<IRepositorioCiclo>();

            consultasCiclo = new ConsultasCiclo(repositorioCiclo.Object);
        }

        [Fact(DisplayName = "DeveObterCiclosFundamentalPorAnoEtapa")]
        public void DeveObterCiclosFundamentalPorAnoEtapa()
        {
            List<string> anos = new List<string>();
            for (int ano = 1; ano <= 9; ano++) anos.Add(ano.ToString());

            repositorioCiclo
                .Setup(rc => rc.ObterCiclosPorAnoModalidade(It.IsAny<FiltroCicloDto>()))
                .Returns(new CicloDto[] { new CicloDto() }.AsEnumerable());

            consultasCiclo.Listar(new FiltroCicloDto { Anos = anos, AnoSelecionado = "1", Modalidade = 5 });
            repositorioCiclo.Verify(c => c.ObterCiclosPorAnoModalidade(It.IsAny<FiltroCicloDto>()), Times.Once);
        }

        [Fact(DisplayName = "DeveObterCiclosMedioPorAnoEtapa")]
        public void DeveObterCiclosMedioPorAnoEtapa()
        {
            List<string> anos = new List<string>();
            for (int ano = 1; ano <= 3; ano++) anos.Add(ano.ToString());

            repositorioCiclo
                .Setup(rc => rc.ObterCiclosPorAnoModalidade(It.IsAny<FiltroCicloDto>()))
                .Returns(new CicloDto[] { new CicloDto() }.AsEnumerable());

            consultasCiclo.Listar(new FiltroCicloDto { Anos = anos, AnoSelecionado = "1", Modalidade = 6 });
            repositorioCiclo.Verify(c => c.ObterCiclosPorAnoModalidade(It.IsAny<FiltroCicloDto>()), Times.Once);
        }

        [Fact(DisplayName = "DeveRetornarErroNegocioAoListarCiclos")]
        public void DeveRetornarErroNegocioAoListarCiclos()
        {
            List<string> anos = new List<string>();
            for (int ano = 1; ano <= 3; ano++) anos.Add(ano.ToString());

            Assert.Equal("Não foi possível localizar o ciclo da turma selecionada", Assert.Throws<NegocioException>(() =>
               consultasCiclo.Listar(new FiltroCicloDto { Anos = anos, AnoSelecionado = "1", Modalidade = 6 })).Message);
        }

        [Fact(DisplayName = "DeveMarcarCicloSelecionadoAoListarCiclos")]
        public void DeveMarcarCicloSelecionadoAoListarCiclos()
        {
            List<string> anos = new List<string>();
            for (int ano = 1; ano <= 3; ano++) anos.Add(ano.ToString());

            repositorioCiclo
                .Setup(rc => rc.ObterCiclosPorAnoModalidade(It.IsAny<FiltroCicloDto>()))
                .Returns(new CicloDto[] { new CicloDto() }.AsEnumerable());

            var ciclos = consultasCiclo.Listar(new FiltroCicloDto { Anos = anos, AnoSelecionado = "1", Modalidade = 6 });
            repositorioCiclo.Verify(c => c.ObterCiclosPorAnoModalidade(It.IsAny<FiltroCicloDto>()), Times.Once);

            Assert.True(ciclos.Any());
            Assert.Single(ciclos);
            Assert.True(ciclos.Single().Selecionado);
        }
    }
}