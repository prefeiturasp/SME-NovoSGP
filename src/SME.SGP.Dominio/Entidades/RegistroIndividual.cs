using System;

namespace SME.SGP.Dominio
{
    public class RegistroIndividual : EntidadeBase
    {
        public RegistroIndividual()
        {
        }

        public long TurmaId { get; set; }
        public long AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public DateTime DataRegistro { get; set; }
        public string Registro { get; set; }
        public bool Migrado { get; set; }
        public bool Excluido { get; set; }

        public void Remover()
        {
            Excluido = true;
        }
    }
}
