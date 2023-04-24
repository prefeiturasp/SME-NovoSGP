using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacoesAusenciasAlunoEAulaPorAulaIdEQuantidadeQueryHandler : IRequestHandler<ObterCompensacoesAusenciasAlunoEAulaPorAulaIdEQuantidadeQuery, IEnumerable<CompensacaoAusenciaAlunoAulaDto>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoConsulta;

        public ObterCompensacoesAusenciasAlunoEAulaPorAulaIdEQuantidadeQueryHandler(IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorio)
        {
            this.repositorioCompensacaoAusenciaAlunoConsulta = repositorio;
        }

        public async Task<IEnumerable<CompensacaoAusenciaAlunoAulaDto>> Handle(ObterCompensacoesAusenciasAlunoEAulaPorAulaIdEQuantidadeQuery request, CancellationToken cancellationToken)
            => await repositorioCompensacaoAusenciaAlunoConsulta.ObterCompensacoesAusenciasAlunoEAulaPorAulaIdTurmaComponenteQuantidade(request.AulaId, request.Quantidade);
    }
}