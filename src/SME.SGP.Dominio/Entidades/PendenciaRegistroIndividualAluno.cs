using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class PendenciaRegistroIndividualAluno
    {
        [Key]
        public long Id { get; set; }
        public long CodigoAluno { get; set; }
        [Computed]
        public PendenciaRegistroIndividual PendenciaRegistroIndividual { get; set; }
        public long PendenciaRegistroIndividualId { get; set; }
        public SituacaoPendenciaRegistroIndividualAluno Situacao { get; set; }

        public PendenciaRegistroIndividualAluno(long codigoAluno, PendenciaRegistroIndividual pendenciaRegistroIndividual)
        {
            SetCodigoAluno(codigoAluno);
            SetPendenciaRegistroIndividual(pendenciaRegistroIndividual);
            Situacao = SituacaoPendenciaRegistroIndividualAluno.Pendente;
        }

        protected PendenciaRegistroIndividualAluno()
        {
        }

        public void ResolverPendenciaDoAluno() => Situacao = SituacaoPendenciaRegistroIndividualAluno.Resolvido;

        private void SetCodigoAluno(long codigoAluno)
        {
            if (codigoAluno == default)
                throw new NegocioException("O código do aluno deve ser informado.");

            CodigoAluno = codigoAluno;
        }

        private void SetPendenciaRegistroIndividual(PendenciaRegistroIndividual pendenciaRegistroIndividual)
        {
            if (pendenciaRegistroIndividual is null)
                throw new NegocioException("A pendência de registro individual deve ser informada.");

            PendenciaRegistroIndividual = pendenciaRegistroIndividual;
            PendenciaRegistroIndividualId = pendenciaRegistroIndividual.Id;
        }
    }
}