using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterRespostaNovoEncaminhamentoNAAPAPorId
{
    public class ObterRespostaNovoEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterRespostaNovoEncaminhamentoNAAPAPorIdQuery, RespostaEncaminhamentoNAAPA>
    {
        public IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA { get; }

        public ObterRespostaNovoEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioRespostaNovoEncaminhamentoNAAPA = repositorioNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPA));
        }

        public Task<RespostaEncaminhamentoNAAPA> Handle(ObterRespostaNovoEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(repositorioRespostaNovoEncaminhamentoNAAPA.ObterPorId(request.Id));
        }
    }
}