using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class PeriodoEscolarTeste
    {
        [Fact]
        public void Deve_Validar_Quantidade_Bimestres_EJA()
        {
            var periodos = new PeriodoEscolarLista();

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.Eja = true;
            periodos.AnoBase = DateTime.Now.Year;

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 1,
                PeriodoFim = DateTime.Now.AddMinutes(1),
                PeriodoInicio = DateTime.Now
            });

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 2,
                PeriodoFim = DateTime.Now.AddMinutes(3),
                PeriodoInicio = DateTime.Now.AddMinutes(2)
            });

            periodos.Validar();
          
        }

        [Fact]
        public void Deve_Validar_Quantidade_Bimestres_Regular()
        {
            var periodos = new PeriodoEscolarLista();

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.AnoBase = DateTime.Now.Year;

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 1,
                PeriodoFim = DateTime.Now.AddMinutes(1),
                PeriodoInicio = DateTime.Now
            });

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 2,
                PeriodoFim = DateTime.Now.AddMinutes(3),
                PeriodoInicio = DateTime.Now.AddMinutes(2)
            });

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 3,
                PeriodoFim = DateTime.Now.AddMinutes(5),
                PeriodoInicio = DateTime.Now.AddMinutes(4)
            });

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 4,
                PeriodoFim = DateTime.Now.AddMinutes(7),
                PeriodoInicio = DateTime.Now.AddMinutes(6)
            });

            periodos.Validar();
        }

        [Fact]
        public void Deve_Validar_Inicio_Apos_Fim_Periodo()
        {
            var periodos = new PeriodoEscolarLista();

            periodos.Eja = true;
            periodos.AnoBase = DateTime.Now.Year;

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 1,
                PeriodoFim = DateTime.Now.AddMinutes(1),
                PeriodoInicio = DateTime.Now
            });

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 2,
                PeriodoFim = DateTime.Now.AddMinutes(3),
                PeriodoInicio = DateTime.Now.AddMinutes(4)
            });

            Assert.Throws<NegocioException>(() => periodos.Validar());
        }

        [Fact]
        public void Deve_Validar_Ano_Base()
        {
            var periodos = new PeriodoEscolarLista();

            periodos.Eja = true;
            periodos.AnoBase = DateTime.Now.Year - 1;

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 1,
                PeriodoFim = DateTime.Now.AddMinutes(1),
                PeriodoInicio = DateTime.Now
            });

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 2,
                PeriodoFim = DateTime.Now.AddMinutes(3),
                PeriodoInicio = DateTime.Now.AddMinutes(2)
            });

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.AnoBase = DateTime.Now.Year + 1;

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.AnoBase = DateTime.Now.Year;

            periodos.Validar();
        }

        [Fact]
        public void Deve_Validar_Inicio_Periodo_Antes_Fim_Periodo_Anterior()
        {
            var periodos = new PeriodoEscolarLista();

            periodos.Eja = true;
            periodos.AnoBase = DateTime.Now.Year;

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 1,
                PeriodoFim = DateTime.Now.AddMinutes(1),
                PeriodoInicio = DateTime.Now
            });

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 2,
                PeriodoFim = DateTime.Now,
                PeriodoInicio = DateTime.Now.AddMinutes(4)
            });

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos = new PeriodoEscolarLista();

            periodos.Eja = false;
            periodos.AnoBase = DateTime.Now.Year;

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 1,
                PeriodoFim = DateTime.Now.AddMinutes(1),
                PeriodoInicio = DateTime.Now
            });

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 2,
                PeriodoFim = DateTime.Now.AddMinutes(3),
                PeriodoInicio = DateTime.Now.AddMinutes(2)
            });

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 3,
                PeriodoFim = DateTime.Now.AddMinutes(3),
                PeriodoInicio = DateTime.Now.AddMinutes(2)
            });

            periodos.Periodos.Add(new PeriodoEscolar
            {
                TipoCalendario = 1,
                Bimestre = 4,
                PeriodoFim = DateTime.Now.AddMinutes(5),
                PeriodoInicio = DateTime.Now.AddMinutes(4)
            });

            Assert.Throws<NegocioException>(() => periodos.Validar());

            periodos.Periodos[2].PeriodoInicio = DateTime.Now.AddMinutes(3);
            periodos.Periodos[2].PeriodoFim = DateTime.Now.AddMinutes(3).AddSeconds(10);

            periodos.Validar();
        }
    }
}
