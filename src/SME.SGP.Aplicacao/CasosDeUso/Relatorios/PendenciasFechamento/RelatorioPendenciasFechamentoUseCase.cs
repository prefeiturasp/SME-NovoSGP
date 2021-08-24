using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioPendenciasFechamentoUseCase : IRelatorioPendenciasUseCase
    {
        private readonly IMediator mediator;

        public RelatorioPendenciasFechamentoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioPendenciasDto filtroRelatorioPendenciasDto)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtroRelatorioPendenciasDto.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtroRelatorioPendenciasDto.UeCodigo));
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtroRelatorioPendenciasDto.UsuarioLogadoNome = usuarioLogado.Nome;
            filtroRelatorioPendenciasDto.UsuarioLogadoRf = usuarioLogado.CodigoRf;
            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Pendencias, filtroRelatorioPendenciasDto, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosPendencias));
        }

        public List<FiltroTipoPendenciaDto> ListarTodosTipos(bool opcaoTodos)
        {
            var listaTipo = new List<FiltroTipoPendenciaDto>();

            if (opcaoTodos)
            {
                var tipos = new FiltroTipoPendenciaDto();
                tipos.Valor = (int)TipoPendenciaGrupo.Todos;
                tipos.Descricao = TipoPendenciaGrupo.Todos.ObterNome();
                listaTipo.Add(tipos);
            }
            var calendario = new FiltroTipoPendenciaDto()
            {
                Valor = (int)TipoPendenciaGrupo.Calendario,
                Descricao = TipoPendenciaGrupo.Calendario.ObterNome()
            };
            listaTipo.Add(calendario);

            var diarioClasse = new FiltroTipoPendenciaDto()
            {
                Valor = (int)TipoPendenciaGrupo.DiarioClasse,
                Descricao = TipoPendenciaGrupo.DiarioClasse.ObterNome()
            };
            listaTipo.Add(diarioClasse);

            var fechamento = new FiltroTipoPendenciaDto()
            {
                Valor = (int)TipoPendenciaGrupo.Fechamento,
                Descricao = TipoPendenciaGrupo.Fechamento.ObterNome()
            };
            listaTipo.Add(fechamento);

            var aee = new FiltroTipoPendenciaDto()
            {
                Valor = (int)TipoPendenciaGrupo.AEE,
                Descricao = TipoPendenciaGrupo.AEE.ObterNome()
            };
            listaTipo.Add(aee);

            return listaTipo;
        }
    }
}
