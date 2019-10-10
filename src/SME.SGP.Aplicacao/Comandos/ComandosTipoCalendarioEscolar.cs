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
        public void Salvar(TipoCalendarioEscolarDto dto)
        {
            var tipoCalendario = MapearParaDominio(dto);
            bool ehRegistroExistente = repositorio.VerificarRegistroExistente(dto.Id, dto.Nome);
            if (ehRegistroExistente)
            {
                throw new NegocioException($"O Tipo de Calendário Escolar '{dto.Nome}' já existe");
            }
            repositorio.Salvar(tipoCalendario);
        }

        public TipoCalendarioEscolar MapearParaDominio(TipoCalendarioEscolarDto dto)
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
