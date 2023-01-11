using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class OcorrenciaDto
    {
        public DateTime DataOcorrencia { get; set; }
        public string HoraOcorrencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string OcorrenciaTipoId { get; set; }
        public bool ConsideraHistorico { get; set; }
        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public long? TurmaId { get; set; }
        public string DreNome { get; set; }
        public string UeNome { get; set; }
        public string ModalidadeNome { get; set; }
        public string TurmaNome { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public IEnumerable<OcorrenciaAlunoDto> Alunos { get; set; }
        public IEnumerable<OcorrenciaServidorDto> Servidores { get; set; }
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }

        public OcorrenciaDto()
        {
            Alunos = new List<OcorrenciaAlunoDto>();
            Servidores = new List<OcorrenciaServidorDto>();
        }
    }
}