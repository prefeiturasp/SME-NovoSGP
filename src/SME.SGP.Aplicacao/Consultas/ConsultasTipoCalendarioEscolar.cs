using System.Collections.Generic;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ConsultasTipoCalendarioEscolar : IConsultasTipoCalendarioEscolar
    {
        private readonly IRepositorioTipoCalendarioEscolar repositorio;

        public ConsultasTipoCalendarioEscolar(IRepositorioTipoCalendarioEscolar repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public IEnumerable<TipoCalendarioEscolarDto> Listar()
        {
            return this.repositorio.ObterTiposCalendarioEscolar();
        }

        public TipoCalendarioEscolarCompletoDto BuscarPorId(long id)
        {
            var entidade = repositorio.ObterPorId(id);
            TipoCalendarioEscolarCompletoDto dto = new TipoCalendarioEscolarCompletoDto();
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
