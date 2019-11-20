using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosEventoTipo : IComandosEventoTipo
    {
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioEvento repositorioEvento;

        public ComandosEventoTipo(IRepositorioEventoTipo repositorioEventoTipo, IUnitOfWork unitOfWork, IRepositorioEvento repositorioEvento)
        {
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public void Alterar(EventoTipoInclusaoDto eventoTipoDto, long idEvento)
        {
            var evento = ObterEntidadeBancoEAtualizar(idEvento, eventoTipoDto);

            repositorioEventoTipo.Salvar(evento);
        }

        public void Remover(IEnumerable<long> idsRemover)
        {
            var idFalhaExclusao = new List<long>();
            var tiposEventoInvalidos = new List<string>();

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var codigo in idsRemover)
                {
                    try
                    {
                        var entidade = repositorioEventoTipo.ObterPorId(codigo);
                        var possuiEventos = repositorioEvento.ExisteEventoPorEventoTipoId(codigo);
                        if (possuiEventos)
                        {
                            tiposEventoInvalidos.Add(entidade.Descricao);
                        }
                        else
                        {
                            entidade.Excluido = true;

                            repositorioEventoTipo.Salvar(entidade);
                        }
                    }
                    catch (Exception)
                    {
                        idFalhaExclusao.Add(codigo);
                    }
                }

                unitOfWork.PersistirTransacao();
            }
            if (tiposEventoInvalidos.Any())
            {
                var erroMensagem = idFalhaExclusao.Count > 1 ?
                    $"Os tipos de evento: {string.Join(",", tiposEventoInvalidos)} não foram removidos, pois possuem eventos vinculados" :
                    $"O tipo de evento {tiposEventoInvalidos[0]} não foi removido, pois possue evento vinculado";

                throw new NegocioException(erroMensagem);
            }

            if (idFalhaExclusao.Any())
            {
                var erroMensagem = idFalhaExclusao.Count > 1 ?
                    $"Os tipos de evento de codigo: {string.Join(",", idFalhaExclusao)} não foram removidos" :
                    $"O tipo de evento de codigo {idFalhaExclusao[0]} não foi removido";

                throw new NegocioException(erroMensagem);
            }
        }

        public void Salvar(EventoTipoInclusaoDto eventoTipoDto)
        {
            var evento = MapearDtoParaDominio(eventoTipoDto);

            repositorioEventoTipo.Salvar(evento);
        }

        private EventoTipo MapearDtoParaDominio(EventoTipoInclusaoDto eventoTipoDto)
        {
            return new EventoTipo
            {
                Concomitancia = eventoTipoDto.Concomitancia,
                Dependencia = eventoTipoDto.Dependencia,
                Descricao = eventoTipoDto.Descricao,
                Letivo = eventoTipoDto.Letivo,
                LocalOcorrencia = eventoTipoDto.LocalOcorrencia,
                TipoData = eventoTipoDto.TipoData
            };
        }

        private EventoTipo ObterEntidadeBancoEAtualizar(long id, EventoTipoInclusaoDto eventoTipoDto)
        {
            var eventoTipo = repositorioEventoTipo.ObterPorId(id);

            if (eventoTipo == null || eventoTipo.Id == 0)
                throw new NegocioException("Não é possivel editar um tipo de evento não cadastrado");

            var possuiEventos = repositorioEvento.ExisteEventoPorEventoTipoId(id);

            if (!possuiEventos)
            {
                eventoTipo.Concomitancia = eventoTipoDto.Concomitancia;
                eventoTipo.Dependencia = eventoTipoDto.Dependencia;
                eventoTipo.Descricao = eventoTipoDto.Descricao;
                eventoTipo.Letivo = eventoTipoDto.Letivo;
                eventoTipo.LocalOcorrencia = eventoTipoDto.LocalOcorrencia;
                eventoTipo.TipoData = eventoTipoDto.TipoData;
            }
            eventoTipo.Ativo = eventoTipoDto.Ativo;

            return eventoTipo;
        }
    }
}