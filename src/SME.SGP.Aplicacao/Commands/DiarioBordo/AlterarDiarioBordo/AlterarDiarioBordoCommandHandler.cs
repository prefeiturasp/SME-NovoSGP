using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDiarioBordoCommandHandler : IRequestHandler<AlterarDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public AlterarDiarioBordoCommandHandler(IMediator mediator,
                                                IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<AuditoriaDto> Handle(AlterarDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var aula = await mediator.Send(new ObterAulaPorIdQuery(request.AulaId));

            if (aula == null)
                throw new NegocioException("Aula informada não existe");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            if (turma == null)
                throw new NegocioException("Turma informada não encontrada");

            if (usuario.EhProfessorCj())
            {
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuario.CodigoRf));

                var atribuicoesEsporadica = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuario.CodigoRf, false, aula.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

                if (possuiAtribuicaoCJ && atribuicoesEsporadica.Any())
                {
                    if (!atribuicoesEsporadica.Where(a => a.DataInicio <= aula.DataAula.Date && a.DataFim >= aula.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe).Any())
                        throw new NegocioException($"Você não possui permissão para alterar o registro de diário de bordo neste período");
                }
            }


            var diarioBordo = await repositorioDiarioBordo.ObterPorAulaId(request.AulaId);
            if (diarioBordo == null)
                throw new NegocioException($"Diário de Bordo para a aula {request.AulaId} não encontrado!");

            await MoverRemoverExcluidos(request, diarioBordo);
            MapearAlteracoes(diarioBordo, request);

            await repositorioDiarioBordo.SalvarAsync(diarioBordo);

            return (AuditoriaDto)diarioBordo;
        }
        private async Task MoverRemoverExcluidos(AlterarDiarioBordoCommand diario, DiarioBordo diarioBordo)
        {
            if (!string.IsNullOrEmpty(diario.Planejamento))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.DiarioBordo, diarioBordo.Planejamento, diario.Planejamento));
                diario.Planejamento = moverArquivo;
            }
            if (!string.IsNullOrEmpty(diarioBordo.Planejamento))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(diarioBordo.Planejamento, diario.Planejamento, TipoArquivo.DiarioBordo.Name()));
            }

            if (!string.IsNullOrEmpty(diario.ReflexoesReplanejamento))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.DiarioBordo, diarioBordo.ReflexoesReplanejamento, diario.ReflexoesReplanejamento));
                diario.ReflexoesReplanejamento = moverArquivo;
            }
            if (!string.IsNullOrEmpty(diarioBordo.ReflexoesReplanejamento))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(diarioBordo.ReflexoesReplanejamento, diario.ReflexoesReplanejamento, TipoArquivo.DiarioBordo.Name()));
            }
        }
        private void MapearAlteracoes(DiarioBordo entidade, AlterarDiarioBordoCommand request)
        {
            entidade.Planejamento = request.Planejamento;
            entidade.ReflexoesReplanejamento = request.ReflexoesReplanejamento;
            entidade.ComponenteCurricularId = request.ComponenteCurricularId;
        }
    }
}
