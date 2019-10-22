using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
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
            consultasCiclo.Listar(new FiltroCicloDto { Anos = anos, AnoSelecionado = "1", Modalidade = 5 });
            repositorioCiclo.Verify(c => c.ObterCiclosPorAnoModalidade(It.IsAny<FiltroCicloDto>()), Times.Once);
        }

        [Fact(DisplayName = "DeveObterCiclosMedioPorAnoEtapa")]
        public void DeveObterCiclosMedioPorAnoEtapa()
        {
            List<string> anos = new List<string>();
            for (int ano = 1; ano <= 3; ano++) anos.Add(ano.ToString());
            consultasCiclo.Listar(new FiltroCicloDto { Anos = anos, AnoSelecionado = "1", Modalidade = 6 });
            repositorioCiclo.Verify(c => c.ObterCiclosPorAnoModalidade(It.IsAny<FiltroCicloDto>()), Times.Once);
        }
    }
}