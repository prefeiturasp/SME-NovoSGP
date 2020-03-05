using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasEvento : ConsultasBase, IConsultasEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasEvento(IRepositorioEvento repositorioEvento,
                               IContextoAplicacao contextoAplicacao,
                               IRepositorioEventoTipo repositorioEventoTipo,
                               IServicoUsuario servicoUsuario) : base(contextoAplicacao)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new System.ArgumentNullException(nameof(repositorioEventoTipo));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<PaginacaoResultadoDto<EventoCompletoDto>> Listar(FiltroEventosDto filtroEventosDto)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            return MapearParaDtoComPaginacao(await repositorioEvento
                .Listar(filtroEventosDto.TipoCalendarioId,
                        filtroEventosDto.TipoEventoId,
                        filtroEventosDto.NomeEvento,
                        filtroEventosDto.DataInicio,
                        filtroEventosDto.DataFim,
                        Paginacao,
                        filtroEventosDto.DreId,
                        filtroEventosDto.UeId,
                        filtroEventosDto.EhTodasDres,
                        filtroEventosDto.EhTodasUes,
                        usuario,
                        usuario.PerfilAtual,
                        usuario.TemPerfilSupervisorOuDiretor(),
                        usuario.PodeVisualizarEventosOcorrenciaDre(),
                        usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme()));
        }

        public async Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            return await repositorioEvento.ObterEventosPorDia(calendarioEventosMesesFiltro, mes, dia, usuario, usuario.PerfilAtual,
                usuario.TemPerfilSupervisorOuDiretor(), usuario.PodeVisualizarEventosOcorrenciaDre(),
                        usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme());
        }

        public async Task<EventoCompletoDto> ObterPorId(long id)
        {
            var evento = repositorioEvento.ObterPorId(id);
            evento.TipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            //verificar se o evento e o perfil do usuário é SME para possibilitar alteração
            bool podeAlterar = !EhEventoSME(evento) || (EhEventoSME(evento) && usuario.EhPerfilSME());

            if (!EhEventoSME(evento) &&
                (evento.TipoEventoId == (long)TipoEvento.LiberacaoExcepcional ||
                 evento.TipoEventoId == (long)TipoEvento.ReposicaoNoRecesso))
                podeAlterar = usuario.TemPerfilGestaoUes();

            return MapearParaDto(evento, podeAlterar);
        }

        public async Task<IEnumerable<CalendarioTipoEventoPorDiaDto>> ObterQuantidadeDeEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            var listaQuery = await repositorioEvento.ObterQuantidadeDeEventosPorDia(calendarioEventosMesesFiltro, mes, usuario, perfilAtual, usuario.TemPerfilSupervisorOuDiretor(),
                usuario.PodeVisualizarEventosOcorrenciaDre(),
                usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme());

            List<CalendarioTipoEventoPorDiaDto> listaRetorno = new List<CalendarioTipoEventoPorDiaDto>();

            if (listaQuery.Any())
            {
                var listaDiasEventos = listaQuery.GroupBy(a => a.Dia).ToList();

                listaDiasEventos.ForEach(a =>
                {
                    var tipoEventos = a.GroupBy(b => b.TipoEvento).Select(b => b.Key).ToList();
                    listaRetorno.Add(new CalendarioTipoEventoPorDiaDto()
                    {
                        Dia = a.Key,
                        TiposEvento = tipoEventos.ToArray(),
                    });
                });
            }

            return listaRetorno;
        }

        public async Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosFiltroDto calendarioEventosMesesFiltro)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            return await repositorioEvento.ObterQuantidadeDeEventosPorMeses(calendarioEventosMesesFiltro, usuario, usuario.PerfilAtual, usuario.PodeVisualizarEventosOcorrenciaDre(),
                        usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme());
        }

        private bool EhEventoSME(Evento evento)
        {
            return evento.UeId == null && evento.DreId == null;
        }

        private IEnumerable<EventoCompletoDto> MapearEventosParaDto(IEnumerable<Evento> items)
        {
            return items?.Select(c => MapearParaDto(c));
        }

        private EventoCompletoDto MapearParaDto(Evento evento, bool? podeAlterar = null)
        {
            return evento == null ? null : new EventoCompletoDto
            {
                DataFim = evento.DataFim,
                DataInicio = evento.DataInicio,
                Descricao = evento.Descricao,
                DreId = evento.DreId,
                FeriadoId = evento.FeriadoId,
                Id = evento.Id,
                Letivo = evento.Letivo,
                Nome = evento.Nome,
                TipoCalendarioId = evento.TipoCalendarioId,
                TipoEventoId = evento.TipoEventoId,
                UeId = evento.UeId,
                AlteradoEm = evento.AlteradoEm,
                AlteradoPor = evento.AlteradoPor,
                AlteradoRF = evento.AlteradoRF,
                CriadoEm = evento.CriadoEm,
                CriadoPor = evento.CriadoPor,
                CriadoRF = evento.CriadoRF,
                TipoEvento = MapearTipoEvento(evento.TipoEvento),
                Migrado = evento.Migrado,
                PodeAlterar = podeAlterar != null ? podeAlterar.Value && !evento.TipoEvento.SomenteLeitura : !evento.TipoEvento.SomenteLeitura,
                Status = evento.Status
            };
        }

        private PaginacaoResultadoDto<EventoCompletoDto> MapearParaDtoComPaginacao(PaginacaoResultadoDto<Evento> eventosPaginados)
        {
            if (eventosPaginados == null)
            {
                eventosPaginados = new PaginacaoResultadoDto<Evento>();
            }
            return new PaginacaoResultadoDto<EventoCompletoDto>
            {
                Items = MapearEventosParaDto(eventosPaginados.Items),
                TotalPaginas = eventosPaginados.TotalPaginas,
                TotalRegistros = eventosPaginados.TotalRegistros
            };
        }

        private async Task<bool> MapearPodeAlterarEventoSMEAsync(Evento evento)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            return !EhEventoSME(evento) || (EhEventoSME(evento) && usuario.EhPerfilSME());
        }

        private EventoTipoDto MapearTipoEvento(EventoTipo tipoEvento)
        {
            return tipoEvento == null ? null : new EventoTipoDto
            {
                Descricao = tipoEvento.Descricao,
                Id = tipoEvento.Id,
                TipoData = tipoEvento.TipoData
            };
        }
    }
}