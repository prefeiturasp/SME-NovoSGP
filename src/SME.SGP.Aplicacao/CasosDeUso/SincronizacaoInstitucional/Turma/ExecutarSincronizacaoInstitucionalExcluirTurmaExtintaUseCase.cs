using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalExcluirTurmaExtintaUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalExcluirTurmaExtintaUseCase
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ExecutarSincronizacaoInstitucionalExcluirTurmaExtintaUseCase(IRepositorioTurma repositorioTurma,IMediator mediator) : base(mediator)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroTurmaCodigoTurmaIdDto>();

            try
            {
                await repositorioTurma.ExcluirTurmaExtintaAsync(filtro.TurmaId);

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível realizar a exclusão da turma extinta {filtro.TurmaCodigo}.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional, ex.Message));
                throw;
            }
        }
    }
}
