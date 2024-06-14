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
        private const int QDADE_MAX_DIAS_DATA_REFERENCIA_EXCLUSAO_AULAS_RECORRENTES = 5;

        public ExcluirAulasRecorrentesTerritorioSaberUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<ExcluirAulasRecorrentesComponenteTerritorioSaberDisponibilizadoFiltro>();
            var dataReferenciaExclusaoAulasRecorrentes = (DateTimeExtension.HorarioBrasilia().Date - filtro.DataReferenciaAtribuicao.Date).Days <= QDADE_MAX_DIAS_DATA_REFERENCIA_EXCLUSAO_AULAS_RECORRENTES
                                                          ? filtro.DataReferenciaAtribuicao.Date
                                                          : DateTimeExtension.HorarioBrasilia().Date;

            var aulasFuturas = await mediator.Send(new ObterAulasFuturasPorDataBaseTurmaComponentesCurricularesQuery(dataReferenciaExclusaoAulasRecorrentes,
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
