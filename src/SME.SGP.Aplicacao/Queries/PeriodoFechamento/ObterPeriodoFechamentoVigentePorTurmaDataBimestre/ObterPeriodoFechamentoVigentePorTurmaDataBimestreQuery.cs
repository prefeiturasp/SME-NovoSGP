using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoVigentePorTurmaDataBimestreQuery : IRequest<PeriodoFechamentoVigenteDto>
    {
        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
        public int Bimestre { get; set; }

        public ObterPeriodoFechamentoVigentePorTurmaDataBimestreQuery(Turma turma, DateTime dataReferencia, int bimestre)
        {
            Turma = turma;
            DataReferencia = dataReferencia;
            Bimestre = bimestre;
        }
    }
}
