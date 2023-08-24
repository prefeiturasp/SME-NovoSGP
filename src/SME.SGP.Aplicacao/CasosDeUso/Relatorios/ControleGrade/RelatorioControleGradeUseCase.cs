﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioControleGradeUseCase : AbstractUseCase, IRelatorioControleGradeUseCase
    {
        public RelatorioControleGradeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioControleGrade filtro)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRf = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ControleGrade, filtro, usuarioLogado,rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosControleGrade));
        }
    }
}
