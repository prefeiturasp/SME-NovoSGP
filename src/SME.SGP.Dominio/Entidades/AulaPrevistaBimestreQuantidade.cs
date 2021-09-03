using System;

namespace SME.SGP.Dominio
{
    public class AulaPrevistaBimestreQuantidade : AulaPrevistaBimestre
    {
        public int CriadasTitular { get; set; }

        public int CriadasCJ { get; set; }

        public int Cumpridas { get; set; }

        public int CumpridasSemFrequencia { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fim { get; set; }

        public int Reposicoes { get; set; }

        public int ReposicoesSemFrequencia { get; set; }

        public bool LancaFrequencia { get; set; }
    }
}
