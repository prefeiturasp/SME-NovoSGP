using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Queries;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using Nest;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AulasSemAtribuicaoSubstituicaoTurmaMensalUseCase : IAulasSemAtribuicaoSubstituicaoTurmaMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas;
        private readonly IMediator mediator;

        public AulasSemAtribuicaoSubstituicaoTurmaMensalUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                                IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas,
                                                                IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioAulas = repositorioAulas ?? throw new ArgumentNullException(nameof(repositorioAulas));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtroTurma = mensagem.ObterObjetoMensagem<FiltroCodigoDataMetricasDto>();
            var turma = await repositorioSGP.ObterTurmaComUeEDrePorCodigo(filtroTurma.Codigo);
            await TratarAtualizacaoMetricasAnteriores(filtroTurma.IgnorarRecheckCargaMetricas, turma, filtroTurma.Data);
            
            if (filtroTurma.Data.FimDeSemana())
                return false;
            if (!await PossuiPeriodoLetivo(turma, filtroTurma.Data))
                return false;

            var codigosComponentesCurricularesTurmaSemAtribuicao = await mediator.Send(new ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQuery(filtroTurma.Codigo, filtroTurma.Data));  
            foreach (var codigoComponente in codigosComponentesCurricularesTurmaSemAtribuicao)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoComponenteMensais, new FiltroComponenteCodigoDataMetricasDto(codigoComponente, filtroTurma.Data, filtroTurma.Codigo)));

            if (codigosComponentesCurricularesTurmaSemAtribuicao.PossuiRegistros())
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoExclusaoTurmaMensais, new FiltroComponentesSemAtribuicaoCodigoDataMetricasDto(codigosComponentesCurricularesTurmaSemAtribuicao.ToArray(), filtroTurma.Data, filtroTurma.Codigo)));
            return true;
        }

        private async Task TratarAtualizacaoMetricasAnteriores(bool ignorarRecheckCargaMetricas, Turma turma, DateTime dataJob)
        {
            if (ignorarRecheckCargaMetricas)
                return;
            if (!(await TratarAtualizacaoMetricasBimestreFechamento(turma, dataJob)))
                if (!(await TratarAtualizacaoMetricasMesAnterior(turma.CodigoTurma, dataJob)))
                    await TratarAtualizacaoMetricasSemanaAnterior(turma.CodigoTurma, dataJob);
        }

        private async Task<bool> TratarAtualizacaoMetricasSemanaAnterior(string codigoTurma, DateTime dataJob)
        {
            if (dataJob.Domingo())
            {
                var dataSegundaFeira = dataJob.AddDays(-6);
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoTurmaMensais, new FiltroCodigoDataMetricasDto(codigoTurma, dataSegundaFeira, true)));
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoTurmaMensais, new FiltroCodigoDataMetricasDto(codigoTurma, dataSegundaFeira.AddDays(1), true)));
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoTurmaMensais, new FiltroCodigoDataMetricasDto(codigoTurma, dataSegundaFeira.AddDays(2), true)));
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoTurmaMensais, new FiltroCodigoDataMetricasDto(codigoTurma, dataSegundaFeira.AddDays(3), true)));
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoTurmaMensais, new FiltroCodigoDataMetricasDto(codigoTurma, dataSegundaFeira.AddDays(4), true)));
                return true;
            }
            return false;
        }

        private async Task<bool> TratarAtualizacaoMetricasMesAnterior(string codigoTurma, DateTime dataJob)
        {
            if (dataJob.Day == 1)
            {
                var primeiroDiaMesAnterior = dataJob.AddMonths(-1);
                var ultimoDiaMesAnterior = new DateTime(primeiroDiaMesAnterior.Year, primeiroDiaMesAnterior.Month, DateTime.DaysInMonth(primeiroDiaMesAnterior.Year, primeiroDiaMesAnterior.Month));
                for (DateTime dataAtual = primeiroDiaMesAnterior; dataAtual <= ultimoDiaMesAnterior; dataAtual = dataAtual.AddDays(1))
                    if (!dataAtual.FimDeSemana())
                        await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoTurmaMensais, new FiltroCodigoDataMetricasDto(codigoTurma, dataAtual, true)));
                return true;
            }
            return false;
        }

        private async Task<bool> TratarAtualizacaoMetricasBimestreFechamento(Turma turma, DateTime dataJob)
        {
            var periodoEscolar = await ObterPeriodoEscolarLetivo(turma, dataJob);
            if (periodoEscolar.EhNulo())
                return false;
            var periodoFechamento = await ObterPeriodoFechamento(periodoEscolar.Id);
            if (periodoFechamento.Fim == dataJob)
            {
                for (DateTime dataAtual = periodoEscolar.Inicio; dataAtual <= periodoEscolar.Fim; dataAtual = dataAtual.AddDays(1))
                    if (!dataAtual.FimDeSemana())
                        await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoTurmaMensais, new FiltroCodigoDataMetricasDto(turma.CodigoTurma, dataAtual, true)));
                return true;
            }
            return false;               
        }

        private async Task<bool> PossuiPeriodoLetivo(Turma turma, DateTime data)
        {
            return (await ObterPeriodoEscolarLetivo(turma, data)).NaoEhNulo();
        }

        private async Task<PeriodoIdDto> ObterPeriodoEscolarLetivo(Turma turma, DateTime data)
        {
            var idTipoCalendario = await repositorioSGP.ObterTipoCalendarioId(turma.AnoLetivo, (int)turma.ModalidadeCodigo.ObterModalidadeTipoCalendario());
            return await repositorioSGP.ObterPeriodoEscolarPorTipoCalendarioData(idTipoCalendario, data);
        }

        private async Task<PeriodoIdDto> ObterPeriodoFechamento(long periodoEscolarId)
        {
            return await repositorioSGP.ObterPeriodoFechamentoPorPeriodoEscolar(periodoEscolarId);
        }
    }
}
