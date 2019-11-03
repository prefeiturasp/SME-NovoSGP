using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ComandosAula : IComandosAula
    {
        private readonly IRepositorioAula repositorio;
        public ComandosAula(IRepositorioAula repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public void Alterar(AulaDto dto, long id)
        {
            var aula = MapearDtoParaEntidade(dto, id);
            repositorio.Salvar(aula);
        }

        public void Excluir(long id)
        {
            var aula = repositorio.ObterPorId(id);
            aula.Excluido = true;
            repositorio.Salvar(aula);
        }

        public void Inserir(AulaDto dto)
        {
            var aula = MapearDtoParaEntidade(dto, 0L);
            repositorio.Salvar(aula);
        }

        private Aula MapearDtoParaEntidade(AulaDto dto, long id)
        {
            Aula aula = new Aula();
            if(id > 0L)
            {
                aula = repositorio.ObterPorId(id);
            }
            aula.Data = dto.Data;
            aula.DisciplinaId = dto.DisciplinaId;
            aula.Quantidade = dto.Quantidade;
            aula.RecorrenciaAula = dto.RecorrenciaAula;
            aula.TipoAula = dto.TipoAula;
            return aula;
        }
    }
}
