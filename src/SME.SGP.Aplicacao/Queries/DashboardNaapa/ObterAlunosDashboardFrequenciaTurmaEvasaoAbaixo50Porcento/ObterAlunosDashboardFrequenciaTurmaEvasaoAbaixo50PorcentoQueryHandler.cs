using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryHandler : ConsultasBase, IRequestHandler<ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery, PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorio;

        public ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioFrequenciaConsulta repositorio) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>> Handle(ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoPaginado(request.Mes, request.AnoLetivo, request.DreCodigo,
                request.UeCodigo, request.TurmaCodigo,
                request.Modalidade, request.Semestre, Paginacao);
        }
    }
}
