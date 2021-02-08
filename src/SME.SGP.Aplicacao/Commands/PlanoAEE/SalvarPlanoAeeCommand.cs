using MediatR;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAeeCommand : IRequest<long>
    {
        public long TurmaId { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public string AlunoNumero { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }
       
        public SalvarPlanoAeeCommand()
        {
        }

        public SalvarPlanoAeeCommand(long turmaId, string alunoNome, string alunoCodigo, string alunoNumero, SituacaoPlanoAEE situacao)
        {
            TurmaId = turmaId;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
            AlunoNumero = alunoNumero;
            Situacao = situacao;
        }
    }
}
