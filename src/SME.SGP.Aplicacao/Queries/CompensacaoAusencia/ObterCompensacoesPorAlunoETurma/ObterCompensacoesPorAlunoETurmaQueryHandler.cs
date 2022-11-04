using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacoesPorAlunoETurmaQueryHandler : IRequestHandler<ObterCompensacoesPorAlunoETurmaQuery, TotalCompensacaoAlunoPorCompensacaoIdDto>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusencia;

        public ObterCompensacoesPorAlunoETurmaQueryHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }
        public async Task<TotalCompensacaoAlunoPorCompensacaoIdDto> Handle(ObterCompensacoesPorAlunoETurmaQuery request, CancellationToken cancellationToken)
         => await repositorioCompensacaoAusencia.ObterTotalCompensacoesECompensacaoIdPorAlunoETurmaAsync(request.Bimestre, request.CodigoAluno, request.DisciplinaCodigo, request.TurmaCodigo);
    }
}
