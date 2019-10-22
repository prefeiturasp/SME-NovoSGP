using SME.SGP.Aplicacao.Interfaces.Comandos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Comandos
{
    public class ComandosEventoTipo : IComandosEventoTipo
    {
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IUnitOfWork unitOfWork;

        public ComandosEventoTipo(IRepositorioEventoTipo repositorioEventoTipo, IUnitOfWork unitOfWork)
        {
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Remover(IEnumerable<long> idsRemover)
        {
            var idFalhaExclusao = new List<long>();

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var codigo in idsRemover)
                {
                    try
                    {
                        var entidade = repositorioEventoTipo.ObterPorId(codigo);

                        entidade.Excluido = true;

                        repositorioEventoTipo.Salvar(entidade);
                    }
                    catch (Exception)
                    {
                        idFalhaExclusao.Add(codigo);
                    }
                }

                unitOfWork.PersistirTransacao();
            }

            if (idFalhaExclusao.Any())
            {
                var erroMensagem = idFalhaExclusao.Count > 1 ?
                    $"Os tipos de evento de codigo: {string.Join(",", idFalhaExclusao)} não foram removidos" :
                    $"O tipo de evento de codigo {idFalhaExclusao[0]} não foi removido";

                throw new NegocioException(erroMensagem);
            }
        }

        public void Salvar(EventoTipoDto eventoTipoDto)
        {
            var evento = eventoTipoDto.Codigo > 0
                ? ObterEntidadeBanco(eventoTipoDto.Codigo, eventoTipoDto)
                : ObterEntidade(eventoTipoDto);

            repositorioEventoTipo.Salvar(evento);
        }

        private EventoTipo ObterEntidade(EventoTipoDto eventoTipoDto)
        {
            return new EventoTipo
            {
                Ativo = eventoTipoDto.Ativo,
                Concomitancia = eventoTipoDto.Concomitancia,
                Dependencia = eventoTipoDto.Dependencia,
                Descricao = eventoTipoDto.Descricao,
                Letivo = eventoTipoDto.Letivo,
                LocalOcorrencia = eventoTipoDto.LocalOcorrencia,
                Id = eventoTipoDto.Codigo,
                TipoData = eventoTipoDto.TipoData,
                Excluido = false
            };
        }

        private EventoTipo ObterEntidadeBanco(long Codigo, EventoTipoDto eventoTipoDto)
        {
            try
            {
                var eventoTipo = repositorioEventoTipo.ObterPorId(Codigo);

                if (eventoTipo == null || eventoTipo.Id == 0)
                    throw new NegocioException("Tipo de evento não existe na base de dados", 404);

                eventoTipo.Ativo = eventoTipoDto.Ativo;
                eventoTipo.Concomitancia = eventoTipoDto.Concomitancia;
                eventoTipo.Dependencia = eventoTipoDto.Dependencia;
                eventoTipo.Descricao = eventoTipoDto.Descricao;
                eventoTipo.Letivo = eventoTipoDto.Letivo;
                eventoTipo.LocalOcorrencia = eventoTipoDto.LocalOcorrencia;
                eventoTipo.TipoData = eventoTipoDto.TipoData;

                return eventoTipo;
            }
            catch (Exception)
            {
                throw new NegocioException("Tipo de evento não existe na base de dados", 404);
            }
        }
    }
}