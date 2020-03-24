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

            return await servicoAula.Salvar(aula, usuario, dto.RecorrenciaAula, aulaOrigemQuantidade);
        }

        public async Task<string> Excluir(long id, string disciplinaNome, RecorrenciaAula recorrencia)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = repositorioAula.ObterCompletoPorId(id);
            aula.DisciplinaNome = disciplinaNome;

            return await servicoAula.Excluir(aula, recorrencia, usuario);
        }

        public async Task<string> Inserir(AulaDto dto)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var aula = MapearDtoParaEntidade(dto, usuario.CodigoRf, usuario.EhProfessorCj());

            return await servicoAula.Salvar(aula, usuario, aula.RecorrenciaAula);
        }

        private Aula MapearDtoParaEntidade(AulaDto dto, string usuarioRf, bool usuarioEhCJ, Aula aula = null)
        {
            Aula entidadeAula = aula ?? new Aula();
            if (string.IsNullOrEmpty(entidadeAula.ProfessorRf))
            {
                entidadeAula.ProfessorRf = usuarioRf;
                // Alteração não muda recorrencia da aula
                entidadeAula.RecorrenciaAula = dto.RecorrenciaAula;
            }
            entidadeAula.UeId = dto.UeId;
            entidadeAula.DisciplinaId = dto.DisciplinaId;
            entidadeAula.DisciplinaNome = dto.DisciplinaNome;
            entidadeAula.DisciplinaCompartilhadaId = dto.DisciplinaCompartilhadaId;
            entidadeAula.DisciplinaNome = dto.DisciplinaNome;
            entidadeAula.TurmaId = dto.TurmaId;
            entidadeAula.TipoCalendarioId = dto.TipoCalendarioId;
            entidadeAula.DataAula = dto.DataAula.Date;
            entidadeAula.Quantidade = dto.Quantidade;
            entidadeAula.TipoAula = dto.TipoAula;
            entidadeAula.AulaCJ = usuarioEhCJ;
            return entidadeAula;
        }
    }
}