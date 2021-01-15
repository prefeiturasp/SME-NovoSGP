using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class InserirOcorrenciaDto
    {
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public long TurmaId { get; set; }
        public IEnumerable<long> CodigosAlunos { get; set; }
        public InserirOcorrenciaDto()
        {
            CodigosAlunos = new List<long>();
        }
    }
}