using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAeeCommand : IRequest<RetornoPlanoAEEDto>
    {
        public long TurmaId { get; set; }
        public int AlunoNumero { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }
        public PlanoAEEPersistenciaDto PlanoAEEDto { get; set; }

        public SalvarPlanoAeeCommand(PlanoAEEPersistenciaDto planoAEEDto, long turmaId, string alunoNome, string alunoCodigo, int alunoNumero)
        {
            PlanoAEEDto = planoAEEDto;
            TurmaId = turmaId;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
            AlunoNumero = alunoNumero;
        }
    }
}
