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
    public class ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQueryHandler : ConsultasBase, IRequestHandler<ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQuery, PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorio;

        public ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioFrequenciaConsulta repositorio) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>> Handle(ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaPaginado(request.Mes, request.FiltroAbrangencia, Paginacao);
        }
    }
}
