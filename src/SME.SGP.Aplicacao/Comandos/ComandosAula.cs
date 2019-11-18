using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAula : IComandosAula
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IServicoAula servicoAula;
        private readonly IServicoUsuario servicoUsuario;

        public ComandosAula(IRepositorioAula repositorio,
                            IServicoUsuario servicoUsuario,
                            IServicoAula servicoAula)
        {
            this.repositorioAula = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoAula = servicoAula ?? throw new ArgumentNullException(nameof(servicoAula));
        }

        public async Task Alterar(AulaDto dto, long id)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = MapearDtoParaEntidade(dto, id, usuario.Id);
            await servicoAula.Salvar(aula, usuario);
        }

        public void Excluir(long id)
        {
            var aula = repositorioAula.ObterPorId(id);
            aula.Excluido = true;
            repositorioAula.Salvar(aula);
        }

        public async Task<string> Inserir(AulaDto dto)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = MapearDtoParaEntidade(dto, 0L, usuario.Id);

            return await servicoAula.Salvar(aula, usuario);
        }

        private Aula MapearDtoParaEntidade(AulaDto dto, long id, long usuarioId)
        {
            Aula aula = new Aula();
            if (id > 0L)
            {
                aula = repositorioAula.ObterPorId(id);
            }
            if (aula.ProfessorId <= 0L)
            {
                aula.ProfessorId = usuarioId;
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