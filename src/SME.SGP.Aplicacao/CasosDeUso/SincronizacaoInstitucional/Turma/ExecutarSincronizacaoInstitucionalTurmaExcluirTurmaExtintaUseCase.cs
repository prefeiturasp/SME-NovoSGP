using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase(IRepositorioTurma repositorioTurma, IMediator mediator) : base(mediator)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroTurmaCodigoTurmaIdDto>();

            if (filtro.EhNulo())
                return false;

            try
            {
                await repositorioTurma.ExcluirTurmaExtintaAsync(filtro.TurmaId);

                return true;
            }
            catch (Exception ex)
            {
                var mensagemErro = $"Não foi possível realizar a exclusão da turma extinta {filtro.TurmaCodigo} / dt atualiz.: {filtro.DataStatusTurmaEscola}{(filtro.DataInicioPeriodo.HasValue ? $" / dt início período: {filtro.DataInicioPeriodo.Value}" : string.Empty)}.";
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagemErro, LogNivel.Negocio, LogContexto.SincronizacaoInstitucional, ex.Message));
                throw;
            }
        }
    }
}