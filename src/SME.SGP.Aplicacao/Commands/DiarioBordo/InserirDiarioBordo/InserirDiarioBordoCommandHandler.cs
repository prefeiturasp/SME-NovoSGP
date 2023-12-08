using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class InserirDiarioBordoCommandHandler : IRequestHandler<InserirDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public InserirDiarioBordoCommandHandler(IMediator mediator,IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<AuditoriaDto> Handle(InserirDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var aula = await mediator.Send(new ObterAulaPorIdQuery(request.AulaId));
            bool inseridoCJ = false;
            
            if(aula.EhNulo())
                throw new NegocioException("Aula informada não existe");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException("Turma informada não encontrada");

            var diarioAulaComponente = await repositorioDiarioBordo.ObterPorAulaId(request.AulaId, request.ComponenteCurricularId);

            if (usuario.EhProfessorCj())
            {
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuario.CodigoRf));

                var atribuicoesEsporadica = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuario.CodigoRf, false, aula.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

                if (possuiAtribuicaoCJ && atribuicoesEsporadica.Any())
                {
                    if (!atribuicoesEsporadica.Any(a => a.DataInicio <= aula.DataAula.Date && a.DataFim >= aula.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe))
                        throw new NegocioException($"Você não possui permissão para inserir registro de diário de bordo neste período");   
                }
                inseridoCJ = true;
            }

            await MoverRemoverExcluidos(request);
            var diarioBordo = MapearParaEntidade(request, turma.Id, inseridoCJ);

            if (diarioAulaComponente.NaoEhNulo())
            {
                diarioBordo.Id = diarioAulaComponente.Id;
                diarioBordo.CriadoEm = diarioAulaComponente.CriadoEm;
                diarioBordo.CriadoPor = diarioAulaComponente.CriadoPor;
                diarioBordo.CriadoRF = diarioAulaComponente.CriadoRF;      
            }

            await repositorioDiarioBordo.SalvarAsync(diarioBordo);

            return (AuditoriaDto)diarioBordo;
        }

        private async Task MoverRemoverExcluidos(InserirDiarioBordoCommand diario)
        {
            if (!string.IsNullOrEmpty(diario.Planejamento))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.DiarioBordo, string.Empty, diario.Planejamento));
                diario.Planejamento = moverArquivo;
            }
        }
        private DiarioBordo MapearParaEntidade(InserirDiarioBordoCommand request, long turmaId, bool inseridoCJ)
            => new DiarioBordo()
            { 
                AulaId = request.AulaId,
                Planejamento = request.Planejamento,
                ComponenteCurricularId = request.ComponenteCurricularId,
                TurmaId = turmaId,
                InseridoCJ = inseridoCJ
            };
    }
}
