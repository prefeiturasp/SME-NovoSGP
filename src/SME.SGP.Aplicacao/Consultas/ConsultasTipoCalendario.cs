using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
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

        public IEnumerable<TipoCalendarioDto> Listar()
        {
            var retorno = repositorio.ObterTiposCalendario();
            return from t in retorno
                   select new TipoCalendarioDto()
                   {
                       Id = t.Id,
                       Nome = t.Nome,
                       AnoLetivo = t.AnoLetivo,
                       Modalidade = t.Modalidade,
                       DescricaoModalidade = t.Modalidade.GetAttribute<DisplayAttribute>().Name
                   };
        }

        public TipoCalendarioCompletoDto BuscarPorId(long id)
        {
            var entidade = repositorio.ObterPorId(id);
            TipoCalendarioCompletoDto dto = new TipoCalendarioCompletoDto();
            if (entidade != null)
            {
                dto.Id = entidade.Id;
                dto.Nome = entidade.Nome;
                dto.AnoLetivo = entidade.AnoLetivo;
                dto.Periodo = entidade.Periodo;
                dto.Modalidade = entidade.Modalidade;
                dto.Situacao = entidade.Situacao;
                dto.AlteradoEm = entidade.AlteradoEm;
                dto.AlteradoPor = entidade.AlteradoPor;
                dto.CriadoRF = entidade.CriadoRF;
                dto.AlteradoRF = entidade.AlteradoRF;
                dto.CriadoEm = entidade.CriadoEm;
                dto.CriadoPor = entidade.CriadoPor;
            }
            return dto;
        }
    }
}
