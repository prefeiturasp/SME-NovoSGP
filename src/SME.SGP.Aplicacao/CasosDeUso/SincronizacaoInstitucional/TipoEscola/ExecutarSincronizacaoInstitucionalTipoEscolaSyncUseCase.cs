using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {

            var tiposEscola = await mediator.Send(new ObterTiposEscolaEolQuery());

            if (tiposEscola == null || !tiposEscola.Any())
            {
                throw new NegocioException("Não foi possível localizar tipos de escolas para a sincronização instituicional");
            }


            foreach (var tipoEscola in tiposEscola)
            {
                try
                {
                    var publicarTratamentoTipoEscola = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalTipoEscolaTratar, tipoEscola, param.CodigoCorrelacao, null));
                    if (!publicarTratamentoTipoEscola)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir o Tipo de Escola : {tipoEscola} na fila de sync.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional));
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            return true;

        }
    }
}
