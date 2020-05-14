using System;

namespace SME.SGP.Infra
{
    public class AulasPrevistasDadasDto
    {
        public int Bimestre { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fim { get; set; }

        public int Cumpridas { get; set; }

        public AulasPrevistasDto Previstas { get; set; }

        public AulasQuantidadePorProfessorDto Criadas { get; set; }

        public int Reposicoes { get; set; }
        public bool PodeEditar { get; set; }
    }
}
