using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoCalendario : IComandosTipoCalendario
    {
        private readonly IRepositorioTipoCalendario repositorio;
        public ComandosTipoCalendario(IRepositorioTipoCalendario repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public void Salvar(TipoCalendarioDto dto)
        {
            var tipoCalendario = MapearParaDominio(dto); 

            bool ehRegistroExistente = repositorio.VerificarRegistroExistente(dto.Id, dto.Nome);
            if (ehRegistroExistente)
            {
                throw new NegocioException($"O Tipo de Calendário Escolar '{dto.Nome}' já existe");
            }
            repositorio.Salvar(tipoCalendario);
        }

        public TipoCalendario MapearParaDominio(TipoCalendarioDto dto)
        {
            TipoCalendario entidade = repositorio.ObterPorId(dto.Id);
            if (entidade == null)
            {
                entidade = new TipoCalendario();
            }
            entidade.Nome = dto.Nome;
            entidade.AnoLetivo = dto.AnoLetivo;
            entidade.Periodo = dto.Periodo;
            entidade.Situacao = dto.Situacao;
            entidade.Modalidade = dto.Modalidade;
            return entidade;
        }

        public void MarcarExcluidos(long[] ids)
        {
            var idsInvalidos = "";
            foreach(long id in ids)
            {
                var tipoCalendario = repositorio.ObterPorId(id);
                if(tipoCalendario != null)
                {
                    tipoCalendario.Excluido = true;
                    repositorio.Salvar(tipoCalendario);
                }
                else {
                    idsInvalidos += idsInvalidos.Equals("") ? $"{id}" : $", {id}";
                }
            }
            if (!idsInvalidos.Trim().Equals(""))
            {
                throw new NegocioException($"Houve um erro ao excluir os tipos de calendário ids '{idsInvalidos}'. Um dos tipos de calendário não existe");
            }
        }
    }
}
