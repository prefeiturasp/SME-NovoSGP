using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisBimestresAlunoQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasFinaisBimestresAlunoQuery(string[] turmasCodigos, string alunoCodigo, DateTime? dataMatricula = null, DateTime? dataSituacao = null, int bimestre = 0)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
        }

        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public DateTime? DataMatricula { get; set; }
        public DateTime? DataSituacao { get; set; }
    }
}
