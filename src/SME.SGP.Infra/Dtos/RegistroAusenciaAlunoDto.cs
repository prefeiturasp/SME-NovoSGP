using System;

namespace SME.SGP.Infra.Dtos
{
    public class RegistroAusenciaAlunoDto
    {
        public string CodigoAluno { get; set; }
        public bool Compareceu { get; set; }
        public DateTime DataAula { get; set; }
        public string NomeAluno { get; set; }
        public int NumeroAula { get; set; }
    }
}