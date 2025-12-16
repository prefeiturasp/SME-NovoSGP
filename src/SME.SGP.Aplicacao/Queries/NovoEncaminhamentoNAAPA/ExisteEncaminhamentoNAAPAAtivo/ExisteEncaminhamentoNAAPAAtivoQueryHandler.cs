using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ExisteEncaminhamentoNAAPAAtivo
{
    public class ExisteEncaminhamentoNAAPAAtivoQueryHandler : IRequestHandler<ExisteEncaminhamentoNAAPAAtivoQuery, bool>
    {
        private readonly IRepositorioNovoEncaminhamentoNAAPA repositorio;

        public ExisteEncaminhamentoNAAPAAtivoQueryHandler(IRepositorioNovoEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(ExisteEncaminhamentoNAAPAAtivoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ExisteEncaminhamentoNAAPAAtivoId(request.EncaminhamentoId);
        }
    }
}