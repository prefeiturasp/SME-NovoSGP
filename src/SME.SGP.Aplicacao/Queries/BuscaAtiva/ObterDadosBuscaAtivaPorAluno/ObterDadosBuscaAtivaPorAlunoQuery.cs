using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosBuscaAtivaPorAlunoQuery : IRequest<DadosBuscaAtivaAlunoDto>
    {
        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }

        public ObterDadosBuscaAtivaPorAlunoQuery(string alunoCodigo, long turmaId, int anoLetivo)
        {
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
        }
    }
}