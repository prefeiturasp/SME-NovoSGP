using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMapeamentoEstudantePorIdQueryHandler : ConsultasBase, IRequestHandler<ObterMapeamentoEstudantePorIdQuery, MapeamentoEstudante>
    {
        private readonly IRepositorioMapeamentoEstudante repositorioMapeamento;
        public ObterMapeamentoEstudantePorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioMapeamentoEstudante repositorioMapeamento) : base(contextoAplicacao)
        {
            this.repositorioMapeamento = repositorioMapeamento ?? throw new ArgumentNullException(nameof(repositorioMapeamento));
        }

        public async Task<MapeamentoEstudante> Handle(ObterMapeamentoEstudantePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioMapeamento.ObterMapeamentoEstudantePorId(request.Id);
        }
    }
}
