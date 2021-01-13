using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PendenciaRegistroIndividual : EntidadeBase
    {
        public Pendencia Pendencia { get; set; }
        public long PendenciaId { get; set; }
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public ICollection<PendenciaRegistroIndividualAluno> Alunos { get; set; }

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
                throw new NegocioException("A pendência deve ser informada.");

            Pendencia = pendencia;
            PendenciaId = pendencia.Id;
        }

        private void SetTurma(Turma turma)
        {
            if (turma is null)
                throw new NegocioException("A turma deve ser informada.");

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

        public void ResolverPendencia()
        {
            Pendencia.Situacao = SituacaoPendencia.Resolvida;
            Pendencia.Excluido = true;
        }
    }
}