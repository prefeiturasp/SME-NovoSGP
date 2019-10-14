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
        public int AnoBase { get; set; }

        private int quantidade => Eja ? 2 : 4;

        public void Validar()
        {
            ValidarQuantidadePeriodos();

            ValidarBimestresRepetidos();

            ValidarPeriodos();

            ValidarInicioPeriodoAntesFimPeriodoAnterior();
        }

        private void ValidarBimestresRepetidos()
        {
            var bimestres = Periodos.Select(x => x.Bimestre).GroupBy(x => x).Where(x => x.Count() > 1);

            if (bimestres.Count() > 0)
                throw new NegocioException("Deve ser informado apenas um periodo por bimestre");
        }

        private void ValidarInicioPeriodoAntesFimPeriodoAnterior()
        {
            for (int i = 1; i < Periodos.Count - 1; i++)
            {
                if (Periodos[i + 1].PeriodoInicio < Periodos[i].PeriodoFim)
                    throw new NegocioException($"O inicio do {i + 1}º Bimestre não pode ser anterior ao fim do {i}º Bimestre");
            }
        }

        private void ValidarPeriodos()
        {
            foreach (var periodo in Periodos)
            {
                periodo.ValidarIncioBimestre();
                periodo.ValidarAnoBase(AnoBase);
            }       
        }

        private void ValidarQuantidadePeriodos()
        {
            bool valido = Periodos.Count == quantidade;

            if(!valido)            
                throw new NegocioException($"Para periodo {(Eja ? "semestral" : "anual")} devem ser informados {quantidade} bimestres");            
        }        

    }
}
