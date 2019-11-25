using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtividadeAvaliativa : IComandosAtividadeAvaliativa
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IServicoUsuario servicoUsuario;

        public ComandosAtividadeAvaliativa(
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IServicoUsuario servicoUsuario)

        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentException(nameof(servicoUsuario));
        }

        public async Task Alterar(AtividadeAvaliativaDto dto, long id)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, id, usuario.CodigoRf);
            await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        public async Task Excluir(long idAtividadeAvaliativa)
        {
            var atividadeAvaliativa = repositorioAtividadeAvaliativa.ObterPorId(idAtividadeAvaliativa);
            if (atividadeAvaliativa is null)
                throw new NegocioException("Não foi possível localizar esta avaliação.");
            atividadeAvaliativa.Excluir();
            await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        public async Task Inserir(AtividadeAvaliativaDto dto)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, 0L, usuario.CodigoRf);
            await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        private AtividadeAvaliativa MapearDtoParaEntidade(AtividadeAvaliativaDto dto, long id, string usuarioRf)
        {
            AtividadeAvaliativa atividadeAvaliativa = new AtividadeAvaliativa();
            if (id > 0L)
            {
                atividadeAvaliativa = repositorioAtividadeAvaliativa.ObterPorId(id);
            }
            if (string.IsNullOrEmpty(atividadeAvaliativa.ProfessorRf))
            {
                atividadeAvaliativa.ProfessorRf = usuarioRf;
            }
            atividadeAvaliativa.UeId = dto.UeId;
            atividadeAvaliativa.DreId = dto.DreId;
            atividadeAvaliativa.TurmaId = dto.TurmaId;
            atividadeAvaliativa.CategoriaId = (int)dto.CategoriaId;
            atividadeAvaliativa.ComponenteCurricularId = dto.ComponenteCurricularId;
            atividadeAvaliativa.TipoAvaliacaoId = dto.TipoAvaliacaoId;
            atividadeAvaliativa.NomeAvaliacao = dto.Nome;
            atividadeAvaliativa.DescricaoAvaliacao = dto.Descricao;
            atividadeAvaliativa.DataAvaliacao = dto.DataAvaliacao;
            return atividadeAvaliativa;
        }
    }
}