using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery : IRequest<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>>
    {
        public ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery(string dreCodigo, string ueCodigo, Modalidade modalidade, int semestre, int mes)
        {
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            Modalidade = modalidade;
            Semestre = semestre;
            Mes = mes;
        }

        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Mes { get; set; }
    }
}
