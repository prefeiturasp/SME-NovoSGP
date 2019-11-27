using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsultaAtividadeAvaliativa : IConsultaAtividadeAvaliativa
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;

        public ConsultaAtividadeAvaliativa(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)

        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public AtividadeAvaliativaCompletaDto ObterPorId(long id)
        {
            return MapearParaDto(repositorioAtividadeAvaliativa.ObterPorId(id));
        }

        private AtividadeAvaliativaCompletaDto MapearParaDto(AtividadeAvaliativa atividadeAvaliativa)
        {
            return atividadeAvaliativa == null ? null : new AtividadeAvaliativaCompletaDto
            {
                CategoriaId = (CategoriaAtividadeAvaliativa)atividadeAvaliativa.CategoriaId,
                DataAvaliacao = atividadeAvaliativa.DataAvaliacao,
                Descricao = atividadeAvaliativa.DescricaoAvaliacao,
                DisciplinaId = atividadeAvaliativa.DisciplinaId,
                DreId = atividadeAvaliativa.DreId,
                UeId = atividadeAvaliativa.UeId,
                Nome = atividadeAvaliativa.NomeAvaliacao,
                TipoAvaliacaoId = atividadeAvaliativa.TipoAvaliacaoId,
                TurmaId = atividadeAvaliativa.TurmaId,
                AlteradoEm = atividadeAvaliativa.AlteradoEm,
                AlteradoPor = atividadeAvaliativa.AlteradoPor,
                AlteradoRF = atividadeAvaliativa.AlteradoRF,
                CriadoEm = atividadeAvaliativa.CriadoEm,
                CriadoPor = atividadeAvaliativa.CriadoPor,
                CriadoRF = atividadeAvaliativa.CriadoRF
            };
        }
    }
}