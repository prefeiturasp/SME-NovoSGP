using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommandHandler : IRequestHandler<ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasseConsolidadoNota repositorioConselhoClasseConsolidadoNota;

        public ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommandHandler(IMediator mediator, IRepositorioConselhoClasseConsolidadoNota repositorioConselhoClasseConsolidadoNota)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioConselhoClasseConsolidadoNota = repositorioConselhoClasseConsolidadoNota ?? throw new System.ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoNota));
        }


        public async Task<bool> Handle(ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var consolidadoNota = await repositorioConselhoClasseConsolidadoNota.ObterConselhoClasseConsolidadoAlunoNotaPorConsolidadoBimestreDisciplinaAsync(request.ConsolidacaoId, request.AlunoNota.Bimestre, request.AlunoNota.DisciplinaId);
                if (consolidadoNota.EhNulo())
                {
                    var consolidadoAlunoNota = new ConselhoClasseConsolidadoTurmaAlunoNota()
                    {
                        Bimestre = request.AlunoNota.Bimestre > 0 ? request.AlunoNota.Bimestre : null,
                        ComponenteCurricularId = request.AlunoNota.DisciplinaId,
                        ConceitoId = request.AlunoNota.ConceitoId,
                        Nota = request.AlunoNota.Nota,
                        ConselhoClasseConsolidadoTurmaAlunoId = request.ConsolidacaoId
                    };
                    await repositorioConselhoClasseConsolidadoNota.SalvarAsync(consolidadoAlunoNota);
                }
                else
                {
                    consolidadoNota.Nota = request.AlunoNota.Nota;
                    consolidadoNota.ConceitoId = request.AlunoNota.ConceitoId;
                    await repositorioConselhoClasseConsolidadoNota.SalvarAsync(consolidadoNota);
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir o consolidado de conselho de classe aluno turma nota - Aluno Codigo: {request.AlunoNota.AlunoCodigo} - Consolidado: {request.ConsolidacaoId}", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }
    }
}
