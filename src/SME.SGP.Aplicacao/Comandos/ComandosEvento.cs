using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SME.SGP.Aplicacao.CasosDeUso.ExecutarGravarRecorrenciaUseCase;

namespace SME.SGP.Aplicacao
{
    public class ComandosEvento : IComandosEvento
    {
        private readonly IRepositorioComunicado repositorioComunicado;
        private readonly IMediator mediator;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IServicoEvento servicoEvento;
        private readonly IServicoWorkflowAprovacao servicoWorkflowAprovacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoAbrangencia servicoAbrangencia;

        public ComandosEvento(IRepositorioEvento repositorioEvento,
                              IRepositorioEventoTipo repositorioEventoTipo,
                              IServicoEvento servicoEvento,
                              IServicoWorkflowAprovacao servicoWorkflowAprovacao,
                              IServicoUsuario servicoUsuario,
                              IServicoAbrangencia servicoAbrangencia,
                              IRepositorioComunicado repositorioComunicado,
                              IMediator mediator)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this.mediator = mediator;
            this.servicoEvento = servicoEvento ?? throw new ArgumentNullException(nameof(servicoEvento));
            this.servicoWorkflowAprovacao = servicoWorkflowAprovacao ?? throw new ArgumentNullException(nameof(servicoWorkflowAprovacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
        }

        public async Task<IEnumerable<RetornoCopiarEventoDto>> Alterar(long id, EventoDto eventoDto)
        {
            var evento = repositorioEvento.ObterPorId(id);

            if (evento == null)
                throw new NegocioException("Não foi possível obter o evento");

            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);

            if (tipoEvento == null)
                throw new NegocioException("Não foi possível obter o tipo do evento");

            try
            {
                ValidacaoPermissaoEdicaoExclusaoPorPerfilUsuarioTipoEevento(evento);
            }
            catch (NegocioException)
            {
                throw new NegocioException($"O seu perfil de usuário não permite a alteração desse evento");
            }

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

        public async Task Excluir(long[] idsEventos)
        {
            List<long> idsComErroAoExcluir = new List<long>();
            IList<string> eventoSemPemissaoExclusao = new List<string>();

            foreach (var idEvento in idsEventos)
            {
                try
                {
                    var evento = repositorioEvento.ObterPorId(idEvento);
                    var existeComunicadoParaEvento = await repositorioComunicado.VerificaExistenciaComunicadoParaEvento(idEvento);
                    if (existeComunicadoParaEvento)
                        throw new NegocioException($"Existem comunicados vigentes vinculados para o evento ID: {idEvento}");

                    ValidacaoPermissaoEdicaoExclusaoPorPerfilUsuarioTipoEevento(evento);

                    if (evento.WorkflowAprovacaoId.HasValue)
                        await servicoWorkflowAprovacao.ExcluirWorkflowNotificacoes(evento.WorkflowAprovacaoId.Value);

                    evento.Excluir();

                    repositorioEvento.Salvar(evento);
                }
                catch (NegocioException nex)
                {
                    eventoSemPemissaoExclusao.Add(nex.Message);
                }
                catch (Exception)
                {
                    idsComErroAoExcluir.Add(idEvento);
                }
            }

            var mensagensErroRetorno = new StringBuilder();

            if (eventoSemPemissaoExclusao.Any())
                mensagensErroRetorno.AppendLine($"Não foi permitida a exclusão do(s) evento(s): { string.Join(", ", eventoSemPemissaoExclusao) }");

            if (idsComErroAoExcluir.Any())
                mensagensErroRetorno.AppendLine($"Não foi possível excluir os eventos de ids {string.Join(",", idsComErroAoExcluir)}");

            if (eventoSemPemissaoExclusao.Any() || idsComErroAoExcluir.Any())
                throw new NegocioException(mensagensErroRetorno.ToString());
        }

        public async Task GravarRecorrencia(EventoDto eventoDto, Evento evento)
        {
            if (eventoDto.RecorrenciaEventos != null)
            {
                var recorrencia = eventoDto.RecorrenciaEventos;
                await servicoEvento.SalvarRecorrencia(evento,
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
            eventoDto.DataInicio = eventoDto.DataInicio.Date;
            eventoDto.DataFim = eventoDto.DataFim.HasValue ? eventoDto.DataFim.Value.Date : eventoDto.DataInicio;
            evento.DataFim = eventoDto.DataFim.Value.Local();
            evento.DataInicio = eventoDto.DataInicio.Date.Local();
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
            evento.ValidarDescricaoEvento();

            var retornoCadasradoEvento = await servicoEvento.Salvar(evento, eventoDto.Bimestre, eventoDto.AlterarARecorrenciaCompleta, eventoDto.DataConfirmada);
            var mensagens = new List<RetornoCopiarEventoDto>
            {
                new RetornoCopiarEventoDto(retornoCadasradoEvento, true)
            };
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ExecutarGravarRecorrencia, new ExecutarGravarRecorrenciaParametro() { Dto = eventoDto, Evento = evento }, Guid.NewGuid(), null));
            mensagens.AddRange(await CopiarEventos(eventoDto));

            return mensagens;
        }

        private void ValidacaoPermissaoEdicaoExclusaoPorPerfilUsuarioTipoEevento(Evento evento)
        {
            var usuario = servicoUsuario.ObterUsuarioLogado().Result;

            if (evento.EhEventoSME() && !usuario.EhPerfilSME())
                throw new NegocioException(evento.Nome);

            if (evento.EhEventoDRE() && ((!usuario.EhPerfilDRE() && !usuario.EhPerfilSME()) || !servicoAbrangencia.DreEstaNaAbrangencia(usuario.Login, usuario.PerfilAtual, evento.DreId)))
                throw new NegocioException(evento.Nome);

            if (evento.EhEventoUE() && ((!usuario.EhPerfilUE() && !usuario.EhPerfilDRE() && !usuario.EhPerfilSME()) || !servicoAbrangencia.UeEstaNaAbrangecia(usuario.Login, usuario.PerfilAtual, evento.DreId, evento.UeId)))
                throw new NegocioException(evento.Nome);
        }
    }
}