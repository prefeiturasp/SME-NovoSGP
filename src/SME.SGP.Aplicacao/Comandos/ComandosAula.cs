using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAula : IComandosAula
    {
        private readonly IRepositorioAula repositorio;
        private readonly IServicoUsuario servicoUsuario;
        public ComandosAula(IRepositorioAula repositorio, IServicoUsuario servicoUsuario)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }
        public async Task Alterar(AulaDto dto, long id)
        {
            var aula = await MapearDtoParaEntidade(dto, id);
            repositorio.Salvar(aula);
        }

        public void Excluir(long id)
        {
            var aula = repositorio.ObterPorId(id);
            aula.Excluido = true;
            repositorio.Salvar(aula);
        }

        public async Task Inserir(AulaDto dto)
        {
            var aula = await MapearDtoParaEntidade(dto, 0L);
            repositorio.Salvar(aula);
        }

        private async Task<Aula> MapearDtoParaEntidade(AulaDto dto, long id)
        {
            Aula aula = new Aula();
            if(id > 0L)
            {
                aula = repositorio.ObterPorId(id);
            }
            if (string.IsNullOrEmpty(aula.ProfessorRf))
            {
                var usuario = await servicoUsuario.ObterUsuarioLogado();
                aula.ProfessorRf = usuario.CodigoRf;
            }
            aula.UeId = dto.UeId;
            aula.DisciplinaId = dto.DisciplinaId;
            aula.TurmaId = dto.TurmaId;
            aula.TipoCalendarioId = dto.TipoCalendarioId;
            aula.DataAula = dto.DataAula;
            aula.Quantidade = dto.Quantidade;
            aula.RecorrenciaAula = dto.RecorrenciaAula;
            aula.TipoAula = dto.TipoAula;
            return aula;
        }
    }
}
