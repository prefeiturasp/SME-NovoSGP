using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoAnoAnteriorPorTurmaBimestreQuery : IRequest<PeriodoFechamentoVigenteDto>
    {
        public Turma Turma { get; set; }
        public int Bimestre { get; set; }

        public ObterPeriodoFechamentoAnoAnteriorPorTurmaBimestreQuery(Turma turma, int bimestre)
        {
            Turma = turma;
            Bimestre = bimestre;
        }
    }
}
