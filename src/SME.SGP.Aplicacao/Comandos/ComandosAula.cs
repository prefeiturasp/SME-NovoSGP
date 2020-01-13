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

        public async Task<string> Alterar(AulaDto dto, long id)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aulaOrigem = repositorioAula.ObterPorId(id);
            var aulaOrigemQuantidade = aulaOrigem.Quantidade;
            var aula = MapearDtoParaEntidade(dto, usuario.CodigoRf, usuario.EhProfessorCj(), aulaOrigem);

            return servicoAula.Salvar(aula, usuario, dto.RecorrenciaAula, aulaOrigemQuantidade);
        }

        public async Task<string> Excluir(long id, RecorrenciaAula recorrencia)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = repositorioAula.ObterPorId(id);

            return await servicoAula.Excluir(aula, recorrencia, usuario);
        }

        public async Task<string> Inserir(AulaDto dto)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = MapearDtoParaEntidade(dto, usuario.CodigoRf, usuario.EhProfessorCj());

            return servicoAula.Salvar(aula, usuario, aula.RecorrenciaAula);
        }

        private Aula MapearDtoParaEntidade(AulaDto dto, string usuarioRf, bool usuarioEhCJ, Aula aula = null)
        {
            Aula aulaEntity = aula ?? new Aula();
            if (string.IsNullOrEmpty(aulaEntity.ProfessorRf))
            {
                aulaEntity.ProfessorRf = usuarioRf;
                // Alteração não muda recorrencia da aula
                aulaEntity.RecorrenciaAula = dto.RecorrenciaAula;
            }
            aulaEntity.UeId = dto.UeId;
            aulaEntity.DisciplinaId = dto.DisciplinaId;
            aulaEntity.TurmaId = dto.TurmaId;
            aulaEntity.TipoCalendarioId = dto.TipoCalendarioId;
            aulaEntity.DataAula = dto.DataAula.Local();
            aulaEntity.Quantidade = dto.Quantidade;
            aulaEntity.TipoAula = dto.TipoAula;
            aulaEntity.AulaCJ = usuarioEhCJ;
            return aulaEntity;
        }
    }
}