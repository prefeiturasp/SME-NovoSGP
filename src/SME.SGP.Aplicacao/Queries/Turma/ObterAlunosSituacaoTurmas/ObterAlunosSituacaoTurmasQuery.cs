using MediatR;
using SME.SGP.Infra.Dtos.SituacaoAluno;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSituacaoTurmasQuery : IRequest<List<AlunosSituacaoTurmas>>
    {
        public ObterAlunosSituacaoTurmasQuery(int anoLetivo, int situacaoMatricula, string codigoDre)
        {
            AnoLetivo = anoLetivo;
            SituacaoMatricula = situacaoMatricula;
            CodigoDre = codigoDre;
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public int SituacaoMatricula { get; set; }
    }
}
