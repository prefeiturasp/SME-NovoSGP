using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterNovoEncaminhamentoNAAPAPorId
{
    public class ObterNovoEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterNovoEncaminhamentoNAAPAPorIdQuery, EncaminhamentoEscolar>
    {
        private readonly IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNaapa;
        public ObterNovoEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNaapa) : base(contextoAplicacao)
        {
            this.repositorioNovoEncaminhamentoNaapa = repositorioNovoEncaminhamentoNaapa ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNaapa));
        }

        public async Task<EncaminhamentoEscolar> Handle(ObterNovoEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioNovoEncaminhamentoNaapa.ObterEncaminhamentoPorId(request.Id);
        }
    }
}