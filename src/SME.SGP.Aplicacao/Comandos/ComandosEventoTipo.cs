using SME.SGP.Aplicacao.Interfaces.Comandos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Comandos
{
    public class ComandosEventoTipo : IComandosEventoTipo
    {
        private IRepositorioEventoTipo repositorioEventoTipo;

        public ComandosEventoTipo(IRepositorioEventoTipo repositorioEventoTipo)
        {
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
        }

        public void Salvar(EventoTipoDto eventoTipoDto)
        {
            EventoTipo evento = eventoTipoDto.Codigo > 0 ? ObterEntidadeBanco(eventoTipoDto.Codigo, eventoTipoDto) : ObterEntidade(eventoTipoDto);

            repositorioEventoTipo.Salvar(evento);
        }

        public void Remover(long Codigo)
        {
            try
            {
                repositorioEventoTipo.Remover(Codigo);
            }
            catch (Exception)
            {
                throw new NegocioException("Tipo de evento não existe na base de dados", 404);
            }
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
                Migrado = false
            };
        }

        private EventoTipo ObterEntidadeBanco(long Codigo, EventoTipoDto eventoTipoDto)
        {
            try
            {
                var eventoTipo = repositorioEventoTipo.ObterPorId(Codigo);

                if(eventoTipo == null || eventoTipo.Id == 0)
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
                throw new NegocioException("Tipo de evento não existe na base de dados",404);
            }
        }
    }
}
