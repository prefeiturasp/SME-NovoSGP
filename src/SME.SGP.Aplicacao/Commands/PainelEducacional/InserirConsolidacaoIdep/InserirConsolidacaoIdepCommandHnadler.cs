using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirConsolidacaoIdepCommandHandler : IRequestHandler<InserirConsolidacaoIdepCommand, bool>
    {
        private readonly IRepositorioIdepPainelEducacionalConsolidacao repositorioIdepConsolidacao;

        public InserirConsolidacaoIdepCommandHandler(IRepositorioIdepPainelEducacionalConsolidacao repositorioIdepConsolidacao)
        {
            this.repositorioIdepConsolidacao = repositorioIdepConsolidacao ?? throw new ArgumentNullException(nameof(repositorioIdepConsolidacao));
        }

        public async Task<bool> Handle(InserirConsolidacaoIdepCommand request, CancellationToken cancellationToken)
        {
            if (request.DadosIdep == null)
                return false;

            await repositorioIdepConsolidacao.Inserir(request.DadosIdep);

            return true;
        }
    }
}