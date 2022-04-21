using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaAulasPorTipoCommandHandler : AsyncRequestHandler<SalvarPendenciaAulasPorTipoCommand>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPendenciaAulasPorTipoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task Handle(SalvarPendenciaAulasPorTipoCommand request, CancellationToken cancellationToken)
        {
            var aulasAgrupadas = request.Aulas.GroupBy(x => new { x.TurmaId, x.DisciplinaId });

            var componentesCurriculares = await mediator.Send(new ObterDescricaoComponentesCurricularesPorIdsQuery(request.Aulas.Select(s => long.Parse(s.DisciplinaId)).Distinct().ToArray()));

            var turmasDreUe = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(request.Aulas.Select(s => s.TurmaId).Distinct().ToArray()));

            foreach (var item in aulasAgrupadas)
            {
                unitOfWork.IniciarTransacao();

                try
                {
                    var turmaComDreUe = turmasDreUe.FirstOrDefault(f => f.CodigoTurma.Equals(item.Key.TurmaId));

                    var salvarPendenciaCommand = new SalvarPendenciaCommand
                    {
                        TipoPendencia = request.TipoPendenciaAula,
                        DescricaoComponenteCurricular = componentesCurriculares.FirstOrDefault(f => f.Id == long.Parse(item.Key.DisciplinaId)).Descricao,
                        TurmaAnoComModalidade = turmaComDreUe.NomeComModalidade(),
                        DescricaoUeDre = $"{ObterEscola(turmaComDreUe)}",
                    };

                    var pendenciaId = await mediator.Send(salvarPendenciaCommand);
                    await mediator.Send(new SalvarPendenciasAulasCommand(pendenciaId, item.Select(x => x.Id)));
                    await SalvarPendenciaUsuario(pendenciaId, item.First().ProfessorRf);
                    
                    unitOfWork.PersistirTransacao();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();

                    throw;
                }
            }
        }

        private string ObterEscola(Turma turmaDreUe)
        {
            var ueTipo = turmaDreUe.Ue.TipoEscola;

            var dreAbreviacao = turmaDreUe.Ue.Dre.Abreviacao.Replace("-", "");

            var ueNome = turmaDreUe.Ue.Nome;

            return ueTipo != TipoEscola.Nenhum ? $"{ueTipo.ShortName()} {ueNome} ({dreAbreviacao})" : $"{ueNome} ({dreAbreviacao})";
        }

        private async Task SalvarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
    }
}
