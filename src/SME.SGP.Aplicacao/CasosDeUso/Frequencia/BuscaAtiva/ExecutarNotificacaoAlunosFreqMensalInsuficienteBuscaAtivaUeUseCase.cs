using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase : AbstractUseCase, IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase
    {
        public ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();
            if (filtro.Data.Day.Equals(DateTime.DaysInMonth(filtro.Data.Year, filtro.Data.Month)))
            {
                var consolidacoesFrequenciaMensalInsuficientes = await mediator.Send(new ObterConsolidacoesFrequenciaAlunoMensalInsuficientesQuery(filtro.Id, filtro.Data.Year, filtro.Data.Month));
                if (consolidacoesFrequenciaMensalInsuficientes.PossuiRegistros())
                {
                    var ue = await mediator.Send(new ObterCodigoUEDREPorIdQuery(filtro.Id));
                    var responsaveis = await ObterResponsaveis(ue.UeCodigo);

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaProfissionaisNAAPA, 
                                                                   new FiltroNotificacaoAlunosFreqMensalInsuficienteBuscaAtiva() 
                                                                   {
                                                                      ConsolidacoesFrequenciaMensalInsuficientes = consolidacoesFrequenciaMensalInsuficientes,
                                                                      ResponsaveisNotificacao = responsaveis
                                                                   },
                                                                   Guid.NewGuid()));
                }
            }
            return true;
        }

        private async Task<IEnumerable<AtribuicaoResponsavelDto>> ObterResponsaveis(string codigoUe)
        {
            var responsaveis = (await mediator.Send(new ObterAtribuicaoResponsaveisPorUeTipoQuery(codigoUe, TipoResponsavelAtribuicao.PsicologoEscolar))).ToList();
            responsaveis.AddRange(await mediator.Send(new ObterAtribuicaoResponsaveisPorUeTipoQuery(codigoUe, TipoResponsavelAtribuicao.Psicopedagogo)));
            responsaveis.AddRange(await mediator.Send(new ObterAtribuicaoResponsaveisPorUeTipoQuery(codigoUe, TipoResponsavelAtribuicao.AssistenteSocial)));
            return responsaveis.Distinct();
        }
    }
}
