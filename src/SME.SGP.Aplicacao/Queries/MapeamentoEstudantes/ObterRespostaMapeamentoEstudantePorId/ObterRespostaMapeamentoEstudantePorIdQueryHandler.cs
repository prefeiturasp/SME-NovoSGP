using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaMapeamentoEstudantePorIdQueryHandler : ConsultasBase, IRequestHandler<ObterRespostaMapeamentoEstudantePorIdQuery, RespostaMapeamentoEstudante>
    {
        public IRepositorioRespostaMapeamentoEstudante repositorioResposta { get; }

        public ObterRespostaMapeamentoEstudantePorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRespostaMapeamentoEstudante repositorioResposta) : base(contextoAplicacao)
        {
            this.repositorioResposta = repositorioResposta ?? throw new ArgumentNullException(nameof(repositorioResposta));
        }

        public async Task<RespostaMapeamentoEstudante> Handle(ObterRespostaMapeamentoEstudantePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioResposta.ObterPorIdAsync(request.Id);
        }
    }
}
