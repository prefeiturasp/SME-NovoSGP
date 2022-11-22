using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteEncaminhamentoNAAPAPorEstudanteQueryHandler : IRequestHandler<ExisteEncaminhamentoNAAPAPorEstudanteQuery, bool>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;

        public ExisteEncaminhamentoNAAPAPorEstudanteQueryHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExisteEncaminhamentoNAAPAPorEstudanteQuery request, CancellationToken cancellationToken)
            => await repositorioEncaminhamentoNAAPA.VerificaSeExisteEncaminhamentoPorAluno(request.CodigoEstudante, request.UeId);
    }
}
