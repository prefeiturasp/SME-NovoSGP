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
            TipoCalendarioEscolar entidade = repositorio.ObterPorId(dto.Id);
            if (entidade == null)
            {
                entidade = new TipoCalendarioEscolar();
            }
            entidade.Nome = dto.Nome;
            entidade.AnoLetivo = dto.AnoLetivo;
            entidade.Periodo = dto.Periodo;
            entidade.Situacao = dto.Situacao;
            entidade.Modalidade = dto.Modalidade;
            return entidade;
        }
    }
}
