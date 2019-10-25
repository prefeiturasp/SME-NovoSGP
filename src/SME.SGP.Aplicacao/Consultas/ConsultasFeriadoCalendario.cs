using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFeriadoCalendario : IConsultasFeriadoCalendario
    {
        private readonly IRepositorioFeriadoCalendario repositorio;

        public ConsultasFeriadoCalendario(IRepositorioFeriadoCalendario repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public FeriadoCalendarioCompletoDto BuscarPorId(long id)
        {
            var entidade = repositorio.ObterPorId(id);
            FeriadoCalendarioCompletoDto dto = new FeriadoCalendarioCompletoDto();
            if (entidade != null)
            {
                dto.Id = entidade.Id;
                dto.Nome = entidade.Nome;
                dto.Tipo = entidade.Tipo;
                dto.DataFeriado = entidade.DataFeriado;
                dto.Ativo = entidade.Ativo;
                dto.Abrangencia = entidade.Abrangencia;
                dto.AlteradoEm = entidade.AlteradoEm;
                dto.AlteradoPor = entidade.AlteradoPor;
                dto.AlteradoRF = entidade.AlteradoRF;
                dto.CriadoEm = entidade.CriadoEm;
                dto.CriadoPor = entidade.CriadoPor;
                dto.CriadoRF = entidade.CriadoRF;
            }
            return dto;
        }

        public IEnumerable<FeriadoCalendarioDto> Listar(FiltroFeriadoCalendarioDto filtro)
        {
            return MapearParaDto(repositorio.ObterFeriadosCalendario(filtro));
        }

        private IEnumerable<FeriadoCalendarioDto> MapearParaDto(IEnumerable<FeriadoCalendario> feriados)
        {
            return feriados?.Select(m => new FeriadoCalendarioDto()
            {
                Id = m.Id,
                Nome = m.Nome,
                Tipo = m.Tipo,
                DataFeriado = m.DataFeriado,
                Ativo = m.Ativo,
                Abrangencia = m.Abrangencia
            });
        }
    }
}