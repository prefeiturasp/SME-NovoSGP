using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisBimestresAlunoQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasFinaisBimestresAlunoQuery(string[] turmasCodigos, string alunoCodigo,
            DateTime? dataMatricula = null, DateTime? dataSituacao = null, int bimestre = 0, bool validaMatricula = true, long? tipoCalendario = null)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
            ValidaMatricula = validaMatricula;
            TipoCalendario = tipoCalendario;
        }

        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public DateTime? DataMatricula { get; set; }
        public DateTime? DataSituacao { get; set; }
        public bool ValidaMatricula { get; set; }
        public long? TipoCalendario { get; set; }
    }
}
