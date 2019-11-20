using System;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class AtribuicaoEsporadicaTeste
    {
        [Fact]
        public void ValidarDataFimAntesDataInicio()
        {
            var atribuicao = ObterObjeto();

            var periodos = new List<PeriodoEscolar> {
                new PeriodoEscolar
                {
                    PeriodoInicio = DateTime.Now.AddDays(-5),
                    PeriodoFim = DateTime.Now.AddDays(7)
                }
            };

            atribuicao.DataFim = DateTime.Now.AddDays(-1);

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));

            atribuicao.DataFim = DateTime.Now.AddDays(1);

            atribuicao.Validar(false, DateTime.Now.Year, periodos);
        }

        [Fact]
        public void ValidarDataFimForaPeriodo()
        {
            var atribuicao = ObterObjeto();

            var periodos = new List<PeriodoEscolar> {
                new PeriodoEscolar
                {
                    PeriodoInicio = DateTime.Now.AddDays(-5),
                    PeriodoFim = DateTime.Now.AddDays(6)
                }
            };

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));

            periodos[0].PeriodoFim = DateTime.Now.AddDays(7);
        }

        [Fact]
        public void ValidarDataInicioForaPeriodo()
        {
            var atribuicao = ObterObjeto();

            var periodos = new List<PeriodoEscolar> { new PeriodoEscolar() };

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));

            periodos[0].PeriodoInicio = DateTime.Now.AddDays(1);
            periodos[0].PeriodoFim = DateTime.Now.AddDays(7);

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));

            periodos[0].PeriodoInicio = DateTime.Now.AddMinutes(-1);

            atribuicao.Validar(false, DateTime.Now.Year, periodos);
        }

        [Fact]
        public void ValidarDataPassada()
        {
            var atribuicao = ObterObjeto();

            var periodos = new List<PeriodoEscolar> {
                new PeriodoEscolar
                {
                    PeriodoInicio = DateTime.Now.AddDays(-5),
                    PeriodoFim = DateTime.Now.AddDays(7)
                }
            };

            atribuicao.DataFim = DateTime.Now.AddDays(-1);
            atribuicao.DataInicio = DateTime.Now.AddDays(-2);

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));

            atribuicao.Validar(true, DateTime.Now.Year, periodos);
        }

        private AtribuicaoEsporadica ObterObjeto()
        {
            return new AtribuicaoEsporadica
            {
                UeId = "1",
                ProfessorRf = "1",
                DreId = "1",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(7),
                Id = 1
            };
        }
    }
}