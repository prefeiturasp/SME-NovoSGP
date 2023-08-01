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

                var consolidacaoFrequenciaTurma = await repositorio.ObterConsolidacaoDashboardPorTurmaETipoPeriodo(request.TurmaId, request.TipoConsolidacao, request.PeriodoInicio, request.PeriodoFim) ?? new ConsolidacaoFrequenciaTurma();
                consolidacaoFrequenciaTurma.QuantidadeAcimaMinimoFrequencia = request.QuantidadeAcimaMinimoFrequencia;
                consolidacaoFrequenciaTurma.QuantidadeAbaixoMinimoFrequencia = request.QuantidadeAbaixoMinimoFrequencia;
                consolidacaoFrequenciaTurma.PeriodoInicio = request.PeriodoInicio;
                consolidacaoFrequenciaTurma.PeriodoFim = request.PeriodoFim;
                consolidacaoFrequenciaTurma.TurmaId = request.TurmaId;
                consolidacaoFrequenciaTurma.TipoConsolidacao = request.TipoConsolidacao;
                consolidacaoFrequenciaTurma.TotalAulas = request.TotalAulas;
                consolidacaoFrequenciaTurma.TotalFrequencias = request.TotalFrequencias;
                consolidacaoFrequenciaTurma.TotalAlunos = request.TotalAlunos;
                
                var id = await repositorio.SalvarConsolidacaoFrequenciaTurma(consolidacaoFrequenciaTurma);

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
