using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacaoConselhoPorIdBimestreCommandHandler : IRequestHandler<ExcluirConsolidacaoConselhoPorIdBimestreCommand, bool>
    {
        private readonly IRepositorioConselhoClasseConsolidado repositorioConsolidacaoConselhoClasse;
        private readonly IRepositorioConselhoClasseConsolidadoNota repositorioConsolidacaoConselhoClasseNota;
        private readonly IMediator mediator;

        public ExcluirConsolidacaoConselhoPorIdBimestreCommandHandler(IRepositorioConselhoClasseConsolidado repositorioConsolidacaoConselhoClasse, 
            IRepositorioConselhoClasseConsolidadoNota repositorioConsolidacaoConselhoClasseNota,
            IMediator mediator)
        {
            this.repositorioConsolidacaoConselhoClasse = repositorioConsolidacaoConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoConselhoClasse));
            this.repositorioConsolidacaoConselhoClasseNota = repositorioConsolidacaoConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoConselhoClasseNota));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(ExcluirConsolidacaoConselhoPorIdBimestreCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorioConsolidacaoConselhoClasseNota.ExcluirConsolidacaoConselhoClasseNotaPorIdsConsolidacaoAlunoEBimestre(request.ConsolidacaoConselhoNotasIds);

                if (request.ConsolidacaoConselhoAlunoTurmaIds.Any()) // aluno sem consolidação no ano
                    await repositorioConsolidacaoConselhoClasse.ExcluirLogicamenteConsolidacaoConselhoClasseAlunoTurmaPorIdConsolidacao(request.ConsolidacaoConselhoAlunoTurmaIds);

                return true;
            }
            catch(Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao excluir consolidação do conselho nos ids: {request.ConsolidacaoConselhoAlunoTurmaIds}", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }
    }
}
