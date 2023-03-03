using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.HistoricoEscolarObservacao
{
    public class ObterHistoricoEscolarObservacaoPorAlunoQueryHandler : IRequestHandler<ObterHistoricoEscolarObservacaoPorAlunoQuery, HistoricoEscolarObservacaoDto>
    {
        private readonly IRepositorioHistoricoEscolarObservacao repositorioHistoricoEscolarObservacao;

        public ObterHistoricoEscolarObservacaoPorAlunoQueryHandler(IRepositorioHistoricoEscolarObservacao repositorioHistoricoEscolarObservacao)
        {
            this.repositorioHistoricoEscolarObservacao = repositorioHistoricoEscolarObservacao ?? throw new ArgumentNullException(nameof(repositorioHistoricoEscolarObservacao));
        }

        public Task<HistoricoEscolarObservacaoDto> Handle(ObterHistoricoEscolarObservacaoPorAlunoQuery request, CancellationToken cancellationToken)
        {
            return repositorioHistoricoEscolarObservacao.ObterPorCodigoAlunoAsync(request.AlunoCodigo);
        }
    }
}
