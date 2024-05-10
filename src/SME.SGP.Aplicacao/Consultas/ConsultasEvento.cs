using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ConsultasEvento : ConsultasBase, IConsultasEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioEventoBimestre repositorioEventoBimestre;

        public ConsultasEvento(IRepositorioEvento repositorioEvento,
                               IContextoAplicacao contextoAplicacao,
                               IServicoUsuario servicoUsuario,
                               IRepositorioEventoTipo repositorioEventoTipo,
                               IRepositorioEventoBimestre repositorioEventoBimestre,
                               IConsultasAbrangencia consultasAbrangencia) : base(contextoAplicacao)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioEventoBimestre = repositorioEventoBimestre ?? throw new ArgumentNullException(nameof(repositorioEventoBimestre));
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
                        usuario.PodeVisualizarEventosLibExcepRepoRecessoGestoresUeDreSme(),
                        filtroEventosDto.ConsideraHistorico,
                        filtroEventosDto.EhEventosTodaRede));
        }

        public async Task<IEnumerable<CalendarioEventosNoDiaRetornoDto>> ObterEventosPorDia(CalendarioEventosFiltroDto calendarioEventosMesesFiltro, int mes, int dia, int anoLetivo)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            return await repositorioEvento.ObterEventosPorDia(calendarioEventosMesesFiltro, mes, dia, anoLetivo, usuario);
        }

        public async Task<EventoCompletoDto> ObterPorId(long id)
        {
            var evento = await repositorioEvento.ObterEventoAtivoPorId(id);

            if (evento.EhNulo())
                return null;

            evento.TipoEvento = await repositorioEventoTipo.ObterPorIdAsync(evento.TipoEventoId);
           
            var bimestres = await repositorioEventoBimestre.ObterEventoBimestres(evento.Id);
              
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            //verificar se o evento e o perfil do usuário é SME para possibilitar alteração
            bool podeAlterar = !EhEventoSME(evento) || (EhEventoSME(evento) && usuario.EhPerfilSME());
            if (!EhEventoSME(evento) &&
                (evento.TipoEventoId == (long)TipoEvento.LiberacaoExcepcional ||
                 evento.TipoEventoId == (long)TipoEvento.ReposicaoNoRecesso))
                podeAlterar = usuario.TemPerfilGestaoUes();

            bool podeAlterarExcluirPorPerfilAbrangencia = false;
            if (!EhEventoSME(evento))
            {
                var abrangencia = await consultasAbrangencia.ObterUes(evento.DreId, null);
                podeAlterarExcluirPorPerfilAbrangencia = abrangencia.Any(x => x.Codigo == evento.UeId);
            }

            return MapearParaDto(evento, podeAlterar, podeAlterarExcluirPorPerfilAbrangencia, bimestres);
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
            return evento.UeId.EhNulo() && evento.DreId.EhNulo();
        }

        private IEnumerable<EventoCompletoDto> MapearEventosParaDto(IEnumerable<Evento> items)
        {
            return items?.Select(c => MapearParaDto(c));
        }

        private EventoCompletoDto MapearParaDto(Evento evento, bool? podeAlterar = null, bool? podeAlterarExcluirPorPerfilAbrangencia = null, int[] bimestres = null)
        {
            if (evento.NaoEhNulo())
                return new EventoCompletoDto
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
                    PodeAlterar = podeAlterar.NaoEhNulo() ? podeAlterar.Value && evento.PodeAlterar() : evento.PodeAlterar(),
                    PodeAlterarExcluirPorPerfilAbrangencia = podeAlterarExcluirPorPerfilAbrangencia.NaoEhNulo() ? podeAlterarExcluirPorPerfilAbrangencia : false,
                    Status = evento.Status,
                    Bimestre = bimestres,
                    DescricaoDreUe = $"{MontarDescricaoDre(evento)} - {MontarDescricaoUe(evento)}"
                };

            return null;
        }

        private string MontarDescricaoDre(Evento evento)
        {
            if (evento.Dre.NaoEhNulo() && !string.IsNullOrEmpty(evento.Dre.Abreviacao))
            {
                return $"{evento.Dre.Abreviacao.Replace("-", ":")}";
            }

            return "DRE: Todas";
        }
        
        private string MontarDescricaoUe(Evento evento)
        {
            if (evento.Ue.NaoEhNulo() && !string.IsNullOrEmpty(evento.Ue.Nome))
            {
                return $"UE: {evento.Ue.Nome} ";
            }
            return "UE: Todas ";
        }

        private PaginacaoResultadoDto<EventoCompletoDto> MapearParaDtoComPaginacao(PaginacaoResultadoDto<Evento> eventosPaginados)
        {
            if (eventosPaginados.EhNulo())
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

        private EventoTipoDto MapearTipoEvento(EventoTipo tipoEvento)
        {
            return tipoEvento.EhNulo() ? null : new EventoTipoDto
            {
                Descricao = tipoEvento.Descricao,
                Id = tipoEvento.Id,
                TipoData = tipoEvento.TipoData,
                Letivo = tipoEvento.Letivo
            };
        }
    }
}