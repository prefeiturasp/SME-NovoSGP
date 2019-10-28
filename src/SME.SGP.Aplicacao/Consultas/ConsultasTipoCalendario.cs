using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasTipoCalendario : IConsultasTipoCalendario
    {
        private readonly IRepositorioTipoCalendario repositorio;

        public ConsultasTipoCalendario(IRepositorioTipoCalendario repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public TipoCalendarioCompletoDto BuscarPorId(long id)
        {
            var entidade = repositorio.ObterPorId(id);

            TipoCalendarioCompletoDto dto = new TipoCalendarioCompletoDto();

            if (entidade != null)            
                dto = EntidadeParaDtoCompleto(entidade);
            
            return dto;
        }

        public TipoCalendarioCompletoDto BuscarPorAnoLetivoEModalidade(int anoLetivo, Modalidade modalidade)
        {
            var entidade = repositorio.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade);

            if(entidade != null)
                return EntidadeParaDtoCompleto(entidade);

            return null;
        }

        public IEnumerable<TipoCalendarioDto> Listar()
        {
            var retorno = repositorio.ObterTiposCalendario();
            return from t in retorno
                   select EntidadeParaDto(t);
        }


        public TipoCalendarioDto EntidadeParaDto(TipoCalendario entidade)
        {
           return new TipoCalendarioDto()
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                AnoLetivo = entidade.AnoLetivo,
                Modalidade = entidade.Modalidade,
                DescricaoPeriodo = entidade.Periodo.GetAttribute<DisplayAttribute>().Name,
                Periodo = entidade.Periodo
            };
        }

        public TipoCalendarioCompletoDto EntidadeParaDtoCompleto(TipoCalendario entidade)
        {
            return new TipoCalendarioCompletoDto
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                AnoLetivo = entidade.AnoLetivo,
                Periodo = entidade.Periodo,
                Modalidade = entidade.Modalidade,
                Situacao = entidade.Situacao,
                AlteradoPor = entidade.AlteradoPor,
                CriadoRF = entidade.CriadoRF,
                AlteradoRF = entidade.AlteradoRF,
                CriadoEm = entidade.CriadoEm,
                CriadoPor = entidade.CriadoPor
            };
        }
    }
}