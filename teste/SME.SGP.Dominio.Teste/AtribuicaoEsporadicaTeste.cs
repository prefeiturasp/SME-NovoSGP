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
                    PeriodoInicio = atribuicao.DataInicio,
                    PeriodoFim = atribuicao.DataFim
                }
            };

            atribuicao.DataFim = atribuicao.DataInicio.AddDays(-1);

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));
        }

        [Fact]
        public void ValidarDataFimForaPeriodo()
        {
            var atribuicao = ObterObjeto();

            var periodos = new List<PeriodoEscolar> {
                new PeriodoEscolar
                {
                    PeriodoInicio = DateTime.Now.AddDays(-5).Date <= new DateTime(DateTime.Now.Year - 1, 12, 31) ? new DateTime(DateTime.Now.Year, 01, 01) : DateTime.Now.AddDays(-5).Date,
                    PeriodoFim = DateTime.Now.AddDays(7).Date >= new DateTime(DateTime.Now.Year, 12, 31) ? new DateTime(DateTime.Now.Year, 12, 30) : DateTime.Now.AddDays(7).Date,
                }
            };

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));
        }

        [Fact]
        public void ValidarDataInicioForaPeriodo()
        {
            var atribuicao = ObterObjeto();

            var periodos = new List<PeriodoEscolar> { new PeriodoEscolar() };

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));

            periodos[0].PeriodoInicio = DateTime.Now.AddDays(1);
            periodos[0].PeriodoFim = DateTime.Now.AddDays(7) > new DateTime(DateTime.Now.Year, 12, 31) ? new DateTime(DateTime.Now.Year, 12, 31) : DateTime.Now.AddDays(7);

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));
        }

        [Fact]
        public void ValidarDataPassada()
        {
            var atribuicao = ObterObjeto();

            var periodos = new List<PeriodoEscolar> {
                new PeriodoEscolar
                {
                    PeriodoInicio = atribuicao.DataInicio.AddDays(-5),
                    PeriodoFim = atribuicao.DataFim
                }
            };

            atribuicao.DataInicio = atribuicao.DataInicio.AddDays(-2);
            atribuicao.DataFim = atribuicao.DataInicio.AddDays(-1);

            Assert.Throws<NegocioException>(() => atribuicao.Validar(false, DateTime.Now.Year, periodos));
        }

        private AtribuicaoEsporadica ObterObjeto()
        {
            var atribuicao = new AtribuicaoEsporadica
            {
                UeId = "1",
                ProfessorRf = "1",
                DreId = "1",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(7) > new DateTime(DateTime.Now.Year, 12, 31) ? new DateTime(DateTime.Now.Year, 12, 31) : DateTime.Now.AddDays(7),
                Id = 1
            };

            var anoAtual = DateTime.Now.Year;
            if (atribuicao.DataFim.Year > anoAtual)
                atribuicao.DataFim = new DateTime(anoAtual, 12, 31);

            return atribuicao;
        }
    }
}