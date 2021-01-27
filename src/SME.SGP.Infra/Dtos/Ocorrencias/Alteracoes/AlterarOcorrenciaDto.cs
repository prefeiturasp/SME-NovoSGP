using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AlterarOcorrenciaDto
    {
        public long Id { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public IEnumerable<long> CodigosAlunos { get; set; }
        public AlterarOcorrenciaDto()
        {
            CodigosAlunos = new List<long>();
        }
    }
}