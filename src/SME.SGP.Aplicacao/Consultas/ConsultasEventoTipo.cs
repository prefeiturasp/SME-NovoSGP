using SME.SGP.Aplicacao.Interfaces.Consultas;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasEventoTipo : IConsultasEventoTipo
    {
        private IRepositorioEventoTipo repositorioEventoTipo;

        public ConsultasEventoTipo(IRepositorioEventoTipo repositorioEventoTipo)
        {
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
        }

        public EventoTipoDto ObtenhaPorCodigo(long codigo)
        {
            return EntidadeParaDto(repositorioEventoTipo.ObterPorId(codigo));
        }

        public IList<EventoTipoDto> Listar(FiltroEventoTipoDto Filtro)
        {
            var listaEventoTipo = repositorioEventoTipo.ListarTipos(Filtro.LocalOcorrencia, Filtro.Letivo, Filtro.Descricao);

            if (listaEventoTipo == null || listaEventoTipo.Count == 0 || listaEventoTipo[0].Id == 0)
                return null;

            return listaEventoTipo.Select(x => EntidadeParaDto(x)).ToList();
        }

        private EventoTipoDto EntidadeParaDto(EventoTipo eventoTipo)
        {
            if (eventoTipo == null || eventoTipo.Id == 0)
                return null;

            return new EventoTipoDto
            {
                Descricao = eventoTipo.Descricao,
                Codigo = eventoTipo.Id,
                Concomitancia = eventoTipo.Concomitancia,
                Dependencia = eventoTipo.Dependencia,
                Letivo = eventoTipo.Letivo,
                Ativo = eventoTipo.Ativo,
                LocalOcorrencia = eventoTipo.LocalOcorrencia,
                TipoData = eventoTipo.TipoData,
                Migrado = eventoTipo.Migrado
            };
        }
    }
}
