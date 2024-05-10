using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterMarcadorAlunoQuery : IRequest<MarcadorFrequenciaDto>
    {
        public ObterMarcadorAlunoQuery(AlunoPorTurmaResposta alunoPorTurmaResposta, DateTime dataReferencia, bool ehInfantil)
        {
            Aluno = alunoPorTurmaResposta;
            DataReferencia = dataReferencia;
            EhInfantil = ehInfantil;
        }

        public AlunoPorTurmaResposta Aluno { get; set; }
        public DateTime DataReferencia { get; set; }
        public bool EhInfantil { get; set; }
    }
}
