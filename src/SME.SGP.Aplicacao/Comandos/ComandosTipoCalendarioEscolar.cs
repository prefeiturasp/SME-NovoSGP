using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoCalendarioEscolar : IComandosTipoCalendarioEscolar
    {
        private readonly IRepositorioTipoCalendarioEscolar repositorio;
        public ComandosTipoCalendarioEscolar(IRepositorioTipoCalendarioEscolar repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public void Salvar(TipoCalendarioEscolarCompletoDto dto)
        {
            var tipoCalendario = MapearParaDominio(dto);
            repositorio.Salvar(tipoCalendario);
        }

        public TipoCalendarioEscolar MapearParaDominio(TipoCalendarioEscolarCompletoDto dto)
        {
            var tipoCalendario = new TipoCalendarioEscolar()
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Periodo = dto.Periodo,
                Situacao = dto.Situacao,
                Modalidade = dto.Modalidade
            };
            return tipoCalendario;
        }
    }
}
