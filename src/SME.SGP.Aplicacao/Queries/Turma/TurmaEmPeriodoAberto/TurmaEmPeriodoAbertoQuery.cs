using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class TurmaEmPeriodoAbertoQuery : IRequest<bool>
    {
        public TurmaEmPeriodoAbertoQuery(Turma turma, DateTime dataReferencia, int bimestre, bool ehAnoLetivo)
        {
            Turma = turma;
            DataReferencia = dataReferencia;
            Bimestre = bimestre;
            EhAnoLetivo = ehAnoLetivo;
        }

        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
        public int Bimestre { get; set; }
        public bool EhAnoLetivo { get; set; }
    }
}
