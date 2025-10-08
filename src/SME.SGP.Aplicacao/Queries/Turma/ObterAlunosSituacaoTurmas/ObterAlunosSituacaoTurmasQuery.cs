using MediatR;
using SME.SGP.Infra.Dtos.SituacaoAluno;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSituacaoTurmasQuery : IRequest<List<AlunosSituacaoTurmas>>
    {
        public ObterAlunosSituacaoTurmasQuery(List<string> codigoTurmas, int anoLetivo, int situacaoMatricula)
        {
            CodigoTurmas = codigoTurmas;
            AnoLetivo = anoLetivo;
            SituacaoMatricula = situacaoMatricula;
        }

        public int AnoLetivo { get; set; }
        public int SituacaoMatricula { get; set; }
        public List<string> CodigoTurmas { get; set; }
    }
}
