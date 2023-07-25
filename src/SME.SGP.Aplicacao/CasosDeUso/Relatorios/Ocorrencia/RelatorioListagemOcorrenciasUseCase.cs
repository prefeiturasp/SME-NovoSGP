using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioListagemOcorrenciasUseCase : AbstractUseCase, IRelatorioListagemOcorrenciasUseCase
    {
        public RelatorioListagemOcorrenciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioListagemOcorrenciasDto param)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery()) ?? throw new NegocioException("Não foi possível localizar o usuário.");
            param.NomeUsuario = usuario.Nome;
            param.CodigoRf = usuario.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ListagemOcorrencias, param, usuario, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosListagemOcorrencias));
        }
    }
}
