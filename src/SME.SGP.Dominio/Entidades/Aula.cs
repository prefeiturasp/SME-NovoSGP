using System;

namespace SME.SGP.Dominio
{
    public class Aula : EntidadeBase, ICloneable
    {
        public Aula AulaPai { get; set; }
        public long? AulaPaiId { get; set; }
        public DateTime DataAula { get; set; }
        public string DisciplinaId { get; set; }
        
        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
        public string ProfessorRf { get; set; }
        public int Quantidade { get; set; }
        public RecorrenciaAula RecorrenciaAula { get; set; }
        public TipoAula TipoAula { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }

        public void AdicionarAulaPai(Aula aula)
        {
            AulaPai = aula ?? throw new NegocioException("É necessário informar uma aula.");
            AulaPaiId = aula.Id;
        }

        public object Clone()
        {
            return new Aula
            {
                AlteradoEm = AlteradoEm,
                AlteradoPor = AlteradoPor,
                AlteradoRF = AlteradoRF,
                CriadoEm = CriadoEm,
                CriadoPor = CriadoPor,
                CriadoRF = CriadoRF,
                Excluido = Excluido,
                Id = Id,
                UeId = UeId,
                AulaPai = AulaPai,
                DisciplinaId = DisciplinaId,
                AulaPaiId = AulaPaiId,
                DataAula = DataAula,
                Migrado = Migrado,
                TipoCalendario = TipoCalendario,
                ProfessorRf = ProfessorRf,
                Quantidade = Quantidade,
                RecorrenciaAula = RecorrenciaAula,
                TipoAula = TipoAula,
                TipoCalendarioId = TipoCalendarioId,
                TurmaId = TurmaId
            };
        }
    }
}