using System;

namespace SME.SGP.Infra
{
    public class OcorrenciasPorAlunoDto
    {
        public DateTime DataOcorrencia { get; set; }
        public string RegistradoPor { get; set; }
        public string Titulo { get; set; }
    }
}