using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AlterarOcorrenciaDto
    {
        public long Id { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public int DreId { get; set; }
        public int UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public long TurmaId { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public IEnumerable<long> CodigosAlunos { get; set; }
        public IEnumerable<string> CodigosServidores { get; set; }
        public AlterarOcorrenciaDto()
        {
            CodigosAlunos = new List<long>();
            CodigosServidores = new List<string>();
        }
    }
}