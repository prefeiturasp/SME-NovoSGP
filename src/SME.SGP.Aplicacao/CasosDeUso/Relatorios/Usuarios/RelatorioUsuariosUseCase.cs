using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioUsuariosUseCase : AbstractUseCase, IRelatorioUsuariosUseCase
    {
        public RelatorioUsuariosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioUsuarios filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtro.NomeUsuario = usuarioLogado.Nome;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Usuarios, filtro, usuarioLogado));
        }
    }
}
