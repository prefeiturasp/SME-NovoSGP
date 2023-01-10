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
    public class ObterRespostaEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterRespostaEncaminhamentoNAAPAPorIdQuery, RespostaEncaminhamentoNAAPA>
    {
        public IRepositorioRespostaEncaminhamentoNAAPA repositorioRespostaEncaminhamentoNAAPA { get; }

        public ObterRespostaEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRespostaEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioRespostaEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<RespostaEncaminhamentoNAAPA> Handle(ObterRespostaEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioRespostaEncaminhamentoNAAPA.ObterPorId(request.Id);
        }
    }
}
