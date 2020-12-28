using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Entidades
{
    public class Ocorrencia : EntidadeBase
    {
        public ICollection<OcorrenciaAluno> Alunos { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
        public TimeSpan? HoraOcorrencia { get; set; }
        public OcorrenciaTipo OcorrenciaTipo { get; set; }
        public long OcorrenciaTipoId { get; set; }
        public string Titulo { get; set; }

        public Ocorrencia()
        {
            Alunos = new List<OcorrenciaAluno>();
        }
    }
}