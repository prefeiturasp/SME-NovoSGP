using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosEvento : IComandosEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IServicoEvento servicoEvento;
        private readonly IServicoWorkflowAprovacao servicoWorkflowAprovacao;

        public ComandosEvento(IRepositorioEvento repositorioEvento,
                              IRepositorioEventoTipo repositorioEventoTipo,
                              IServicoEvento servicoEvento,
                              IServicoWorkflowAprovacao servicoWorkflowAprovacao)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.servicoEvento = servicoEvento ?? throw new ArgumentNullException(nameof(servicoEvento));
            this.servicoWorkflowAprovacao = servicoWorkflowAprovacao ?? throw new ArgumentNullException(nameof(servicoWorkflowAprovacao));
        }

        public async Task<IEnumerable<RetornoCopiarEventoDto>> Alterar(long id, EventoDto eventoDto)
        {
            var evento = repositorioEvento.ObterPorId(id);

            if (evento == null)
                throw new NegocioException("Não foi possível obter o evento");

            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);

            if (tipoEvento == null)
                throw new NegocioException("Não foi possível obter o tipo do evento");

            evento.AdicionarTipoEvento(tipoEvento);

            if (!evento.PodeAlterar())
                throw new NegocioException("Não é possível editar um evento em aprovação");

            evento = MapearParaEntidade(evento, eventoDto);
            return await SalvarEvento(eventoDto, evento);
        }

        public async Task<IEnumerable<RetornoCopiarEventoDto>> Criar(EventoDto eventoDto)
        {
            var evento = MapearParaEntidade(new Evento(), eventoDto);
            return await SalvarEvento(eventoDto, evento);
        }

        public void Excluir(long[] idsEventos)
        {
            List<long> idsComErroAoExcluir = new List<long>();

            foreach (var idEvento in idsEventos)
            {
                try
                {
                    var evento = repositorioEvento.ObterPorId(idEvento);

                    if (evento.WorkflowAprovacaoId.HasValue)
                        servicoWorkflowAprovacao.ExcluirWorkflowNotificacoes(evento.WorkflowAprovacaoId.Value);

                    evento.Excluir();

                    repositorioEvento.Salvar(evento);
                }
                catch (Exception)
                {
                    idsComErroAoExcluir.Add(idEvento);
                }
            }

            if (idsComErroAoExcluir.Any())
                throw new NegocioException($"Não foi possível excluir os eventos de ids {string.Join(",", idsComErroAoExcluir)}");
        }

        public void GravarRecorrencia(EventoDto eventoDto, Evento evento)
        {
            if (eventoDto.RecorrenciaEventos != null)
            {
                var recorrencia = eventoDto.RecorrenciaEventos;
                servicoEvento.SalvarRecorrencia(evento,
                                                recorrencia.DataInicio,
                                                recorrencia.DataFim,
                                                recorrencia.DiaDeOcorrencia,
                                                recorrencia.DiasDaSemana,
                                                recorrencia.Padrao,
                                                recorrencia.PadraoRecorrenciaMensal,
                                                recorrencia.RepeteACada);
            }
        }

        private async Task<IEnumerable<RetornoCopiarEventoDto>> CopiarEventos(EventoDto eventoDto)
        {
            var mensagens = new List<RetornoCopiarEventoDto>();
            if (eventoDto.TiposCalendarioParaCopiar != null && eventoDto.TiposCalendarioParaCopiar.Any())
            {
                foreach (var tipoCalendario in eventoDto.TiposCalendarioParaCopiar)
                {
                    eventoDto.TipoCalendarioId = tipoCalendario.TipoCalendarioId;
                    try
                    {
                        var eventoParaCopiar = MapearParaEntidade(new Evento(), eventoDto);
                        await servicoEvento.Salvar(eventoParaCopiar);

                        mensagens.Add(new RetornoCopiarEventoDto($"Evento copiado para o calendário: '{tipoCalendario.NomeCalendario}'.", true));
                    }
                    catch (NegocioException nex)
                    {
                        mensagens.Add(new RetornoCopiarEventoDto($"Erro ao copiar para o calendário: '{tipoCalendario.NomeCalendario}'. {nex.Message}"));
                    }
                }
            }
            return mensagens;
        }

        private Evento MapearParaEntidade(Evento evento, EventoDto eventoDto)
        {
            evento.DataFim = eventoDto.DataFim.HasValue ? eventoDto.DataFim.Value.Local() : eventoDto.DataInicio.Local();
            evento.DataInicio = eventoDto.DataInicio.Local();
            evento.Descricao = eventoDto.Descricao;
            evento.DreId = eventoDto.DreId;
            evento.FeriadoId = eventoDto.FeriadoId;
            evento.Letivo = eventoDto.Letivo;
            evento.Nome = eventoDto.Nome;
            evento.TipoCalendarioId = eventoDto.TipoCalendarioId;
            evento.TipoEventoId = eventoDto.TipoEventoId;
            evento.UeId = eventoDto.UeId;
            return evento;
        }

        private async Task<IEnumerable<RetornoCopiarEventoDto>> SalvarEvento(EventoDto eventoDto, Evento evento)
        {
            var retornoCadasradoEvento = await servicoEvento.Salvar(evento, eventoDto.AlterarARecorrenciaCompleta, eventoDto.DataConfirmada);
            var mensagens = new List<RetornoCopiarEventoDto>
            {
                new RetornoCopiarEventoDto(retornoCadasradoEvento, true)
            };
            Background.Core.Cliente.Executar<IComandosEvento>(a => a.GravarRecorrencia(eventoDto, evento));
            mensagens.AddRange(await CopiarEventos(eventoDto));

            return mensagens;
        }
    }
}