using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class PeriodoEscolarLista
    {
        public PeriodoEscolarLista()
        {
            Periodos = new List<PeriodoEscolar>();
        }

        public List<PeriodoEscolar> Periodos { get; set; }
        public bool Eja { get; set; }

        private int quantidade => Eja ? 2 : 4;

        public void Validar()
        {
            ValidarQuantidadePeriodos();

            ValidarBimestresRepetidos();

            ValidarInicioAposFim();

            ValidarInicioPeriodoAntesFimPeriodoAnterior();
        }

        private void ValidarBimestresRepetidos()
        {
            var bimestres = Periodos.Select(x => x.Bimestre).GroupBy(x => x).Where(x => x.Count() > 1);            
        }

        private void ValidarInicioPeriodoAntesFimPeriodoAnterior()
        {
            for (int i = 1; i < Periodos.Count - 1; i++)
            {
                if (Periodos[i + 1].PeriodoInicio < Periodos[i].PeriodoFim)
                    throw new NegocioException($"O inicio do {i + 1}º Bimestre não pode ser anterior ao fim do {i}º Bimestre");
            }
        }

        private void ValidarInicioAposFim()
        {
            foreach (var periodo in Periodos)            
                periodo.Validar();            
        }

        private void ValidarQuantidadePeriodos()
        {
            bool valido = Periodos.Count == quantidade;

            if(!valido)            
                throw new NegocioException($"Para periodo {(Eja ? "semestral" : "anual")} devem ser informados {quantidade} bimestres");            
        }        

    }
}
