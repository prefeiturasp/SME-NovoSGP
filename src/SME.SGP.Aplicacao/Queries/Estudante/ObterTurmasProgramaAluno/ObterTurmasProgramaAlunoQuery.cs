using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasProgramaAlunoQuery : IRequest<IEnumerable<AlunoTurmaProgramaDto>>
    {

        public ObterTurmasProgramaAlunoQuery(string codigoAluno, int? anoLetivo, bool filtrarSituacaoMatricula = true)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
            FiltrarSituacaoMatricula = filtrarSituacaoMatricula;
        }
        public string CodigoAluno { get; set; }
        public int? AnoLetivo { get; set; }
        public bool FiltrarSituacaoMatricula { get; set; }
    }
}