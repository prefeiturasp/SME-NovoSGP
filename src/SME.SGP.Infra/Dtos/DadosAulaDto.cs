using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class DadosAulaDto
    {
        public bool PodeCadastrarAvaliacao { get; set; }
        public List<AtividadeAvaliativa> Atividade { get; set; }
        public string Disciplina { get; set; }
        public string DisciplinaCompartilhada { get; set; }
        public long? DisciplinaCompartilhadaId { get; set; }
        public long? DisciplinaId { get; set; }
        public bool EhCompartilhada { get; set; }
        public bool EhRegencia { get; set; }
        public bool DentroPeriodo { get; set; }
        public string Horario { get; set; }
        public string Modalidade { get; set; }
        public bool PermiteRegistroFrequencia { get; set; }
        public string Tipo { get; set; }
        public string Turma { get; set; }
        public string UnidadeEscolar { get; set; }
    }
}