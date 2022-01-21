using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTurmaCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalTurmaCommand, bool>
    {
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IMediator mediator;

        public TrataSincronizacaoInstitucionalTurmaCommandHandler(IRepositorioTurma repositorioTurma,
                                                                  IMediator mediator)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(TrataSincronizacaoInstitucionalTurmaCommand request, CancellationToken cancellationToken)
        {
            var turmaEOL = request.TurmaEOL;

            //TODO: BUSCAR DADOS DO USUÁRIO LOGADO PARA PODER PASSAR O RF DO PROFESSOR NO COMMAND DA LINHA 41.
            var codigoRf = "7941706";
            //var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (request.TurmaSGP == null)
                return await IncluirTurmaAsync(turmaEOL, request.TurmaSGP);

            if (turmaEOL.Situacao == "C")
            {
                var turmaAtualizadaComSucesso = await AtualizarTurmaParaHistoricaAsync(turmaEOL.Codigo.ToString());

                if (turmaAtualizadaComSucesso)
                    return await mediator.Send(new TrataAbrangenciaHistoricaTurmaCommand(request.TurmaSGP.AnoLetivo,codigoRf));
            }

            if (turmaEOL.Situacao == "E")
                return await VerificarTurmaExtintaAsync(turmaEOL, request.TurmaSGP.Id);

            if (turmaEOL.Situacao == "O" || turmaEOL.Situacao == "A")
                return await IncluirTurmaAsync(turmaEOL, request.TurmaSGP);

            return true;
        }              

        private async Task<bool> VerificarTurmaExtintaAsync(TurmaParaSyncInstitucionalDto turma, long turmaSgpId)
        {
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.CodigoModalidade, anoAtual, turma.Semestre));

            if (tipoCalendarioId > 0)
            {
                var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
                if (periodosEscolares != null && periodosEscolares.Any())
                {
                    var primeiroPeriodo = periodosEscolares.OrderBy(x => x.Bimestre).First();

                    if (turma.DataStatusTurmaEscola.Date < primeiroPeriodo.PeriodoInicio.Date)
                    {
                        await ExcluirTurnaAsync(turma.Codigo.ToString(), turmaSgpId);
                        return true;
                    }
                    else
                    {
                        return await repositorioTurma.AtualizarTurmaSincronizacaoInstitucionalAsync(turma, true);
                    }
                }
                else
                {
                    await ExcluirTurnaAsync(turma.Codigo.ToString(), turmaSgpId);
                    return true;
                }
            }
            else
            {
                await ExcluirTurnaAsync(turma.Codigo.ToString(), turmaSgpId);
                return true;
            }
        }

        private async Task<bool> AtualizarTurmaParaHistoricaAsync(string turmaId)
        {
            var turmaAtualizada = await repositorioTurma.AtualizarTurmaParaHistorica(turmaId);

            if (!turmaAtualizada)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao atualizar a turma para histórica.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional, "Atualizar Turma Para Historica Async"));
                return turmaAtualizada;
            }

            return turmaAtualizada;
        }

        private async Task<bool> IncluirTurmaAsync(TurmaParaSyncInstitucionalDto turmaEol, Turma turmaSgp)
        {
            if (turmaSgp == null)
            {
                var ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(turmaEol.UeCodigo));

                if (ue == null)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível Incluir a turma de código {turmaEol.Codigo}. Pois não foi encontrado a UE {turmaEol.UeCodigo}.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional, "Atualizar Turma Para Historica Async"));
                    return false;
                }
                await repositorioTurma.SalvarAsync(turmaEol, ue.Id);
            }
            else
            {
                if (turmaSgp.Nome != turmaEol.NomeTurma ||
                   turmaSgp.Ano != turmaEol.Ano ||
                   (int)turmaSgp.TipoTurma != turmaEol.TipoTurma ||
                   turmaSgp.AnoLetivo != turmaEol.AnoLetivo ||
                   turmaSgp.ModalidadeCodigo != turmaEol.CodigoModalidade ||
                   turmaSgp.Semestre != turmaEol.Semestre ||
                   turmaSgp.QuantidadeDuracaoAula != turmaEol.DuracaoTurno ||
                   turmaSgp.TipoTurno != turmaEol.TipoTurno ||
                   turmaSgp.EnsinoEspecial != turmaEol.EnsinoEspecial ||
                   turmaSgp.EtapaEJA != turmaEol.EtapaEJA ||
                   turmaSgp.SerieEnsino != turmaEol.SerieEnsino ||
                   turmaSgp.DataInicio.HasValue != turmaEol.DataInicioTurma.HasValue ||
                   (turmaSgp.DataInicio.HasValue && turmaEol.DataInicioTurma.HasValue && turmaSgp.DataInicio.Value.Date != turmaEol.DataInicioTurma.Value.Date) ||
                   turmaSgp.DataFim.HasValue != turmaEol.DataFim.HasValue ||
                   (turmaSgp.DataFim.HasValue && turmaEol.DataFim.HasValue && turmaSgp.DataFim.Value.Date != turmaEol.DataFim.Value.Date) ||
                   turmaSgp.NomeFiltro != turmaEol.NomeFiltro)


                {
                    await repositorioTurma.AtualizarTurmaSincronizacaoInstitucionalAsync(turmaEol);
                }

            }
            return true;
        }

        private async Task ExcluirTurnaAsync(string turmaCodigo, long turmaId)
            => await repositorioTurma.ExcluirTurmaExtintaAsync(turmaCodigo, turmaId);
    }
}
