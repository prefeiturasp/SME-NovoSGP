using System.Collections.Generic;

namespace SME.SGP.Dominio.Entidades
{
    public class PendenciaRegistroIndividual : EntidadeBase
    {
        public Pendencia Pendencia { get; private set; }
        public long PendenciaId { get; private set; }
        public Turma Turma { get; private set; }
        public long TurmaId { get; private set; }
        public ICollection<PendenciaRegistroIndividualAluno> Alunos { get; private set; }

        public PendenciaRegistroIndividual(Pendencia pendencia, Turma turma)
        {
            SetPendencia(pendencia);
            SetTurma(turma);
        }

        protected PendenciaRegistroIndividual()
        {
        }

        private void SetPendencia(Pendencia pendencia)
        {
            if (pendencia is null)
                throw new NegocioException("");

            Pendencia = pendencia;
            PendenciaId = pendencia.Id;
        }

        private void SetTurma(Turma turma)
        {
            if (turma is null)
                throw new NegocioException("");

            Turma = turma;
            TurmaId = turma.Id;
        }

        public void AdicionarAlunos(IEnumerable<long> codigosAlunos)
        {
            foreach (var codigoAluno in codigosAlunos)
                AdicionarAluno(codigoAluno);
        }

        private void AdicionarAluno(long codigoAluno)
        {
            var aluno = new PendenciaRegistroIndividualAluno(codigoAluno, this);
            Alunos = Alunos ?? new List<PendenciaRegistroIndividualAluno>();
            Alunos.Add(aluno);
        }
    }
}