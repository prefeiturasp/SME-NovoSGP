using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioNotasEConceitosFinaisUseCase : IRelatorioNotasEConceitosFinaisUseCase
    {
        private readonly IMediator mediator;

        public RelatorioNotasEConceitosFinaisUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioNotasEConceitosFinaisDto filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado == null)
                throw new NegocioException("Não foi possível localizar o usuário.");

            filtro.UsuarioNome = usuarioLogado.Nome;
            filtro.UsuarioRf = usuarioLogado.CodigoRf;

            if (filtro.Bimestres.Any(c => c == -99))
            {
                filtro.Bimestres = new List<int>();
                switch (filtro.Modalidade)
                {
                    case Modalidade.Fundamental:
                    case Modalidade.Medio:
                        filtro.Bimestres.AddRange(new int[] { 0, 1, 2, 3, 4 });
                        break;
                    case Modalidade.EJA:
                        filtro.Bimestres.AddRange(new int[] { 0, 1, 2 });
                        break;
                }
            }

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.NotasEConceitosFinais, filtro, usuarioLogado, formato: filtro.TipoFormatoRelatorio, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosNotasConceitosFinais));
        }
    }
}