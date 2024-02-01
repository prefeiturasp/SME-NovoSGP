using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulasRecorrentesTerritorioSaberUseCase : AbstractUseCase, IExcluirAulasRecorrentesTerritorioSaberUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public ExcluirAulasRecorrentesTerritorioSaberUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<ExcluirAulasRecorrentesComponenteTerritorioSaberDisponibilizadoFiltro>();
            var aulasFuturas = await mediator.Send(new ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery(filtro.DataReferenciaAtribuicao,
                                                                                                                     filtro.CodigoTurma,
                                                                                                                     filtro.CodigosComponentesCurricularesDisponibilizados));
            unitOfWork.IniciarTransacao();
            try
            {
                foreach (var aula in aulasFuturas)
                    await mediator.Send(new ExcluirAulaFuturaTerritorioDisponibilizadoCommand(aula.Id));
                unitOfWork.PersistirTransacao();
                return true;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }

    public class ExcluirAulasRecorrentesComponenteTerritorioSaberDisponibilizadoFiltro
    {
        public string[] CodigosComponentesCurricularesDisponibilizados { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime DataReferenciaAtribuicao { get; set; }
    }
}
