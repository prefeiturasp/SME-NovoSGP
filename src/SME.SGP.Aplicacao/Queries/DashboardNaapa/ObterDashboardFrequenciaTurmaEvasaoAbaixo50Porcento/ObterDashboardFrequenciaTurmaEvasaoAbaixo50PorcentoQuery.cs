﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery : IRequest<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>>
    {
        public ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery(int anoLetivo, string dreCodigo, string ueCodigo,
            Modalidade modalidade, int semestre, int mes)
        {
            AnoLetivo = anoLetivo;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            Modalidade = modalidade;
            Semestre = semestre;
            Mes = mes;
        }

        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Mes { get; set; }
    }
}
