using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteEncaminhamentoAEEPorEstudanteQueryHandler : IRequestHandler<ExisteEncaminhamentoAEEPorEstudanteQuery, bool>
    {
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public ExisteEncaminhamentoAEEPorEstudanteQueryHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<bool> Handle(ExisteEncaminhamentoAEEPorEstudanteQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoAEE.VerificaSeExisteEncaminhamentoPorAluno(request.CodigoEstudante);
        }
    }
}
