using System;
using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class
        ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQuery : IRequest<
            IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQuery(string[] turmasCodigo, int bimestre,
            DateTime? dataMatricula = null, DateTime? dataSituacao = null)
        {
            TurmasCodigos = turmasCodigo;
            Bimestre = bimestre;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
        }

        public string[] TurmasCodigos { get; }
        public int Bimestre { get; }
        public DateTime? DataMatricula { get; }
        public DateTime? DataSituacao { get; }
    }
}