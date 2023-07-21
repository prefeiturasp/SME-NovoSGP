using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoFrequenciaTurmaCommandHandler : IRequestHandler<RegistraConsolidacaoFrequenciaTurmaCommand, long>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorio;
        private readonly IUnitOfWork unitOfWork;

        public RegistraConsolidacaoFrequenciaTurmaCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorio,
                                                                 IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<long> Handle(RegistraConsolidacaoFrequenciaTurmaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                unitOfWork.IniciarTransacao();

                await repositorio.Excluir(request.TurmaId);
                var id = await repositorio
                    .Inserir(new ConsolidacaoFrequenciaTurma(request.TurmaId, request.QuantidadeAcimaMinimoFrequencia, request.QuantidadeAbaixoMinimoFrequencia, request.TipoConsolidado));

                unitOfWork.PersistirTransacao();

                return id;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }            
        }
    }
}
