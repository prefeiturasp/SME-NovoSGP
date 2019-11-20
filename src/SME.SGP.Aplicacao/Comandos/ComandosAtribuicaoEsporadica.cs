using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtribuicaoEsporadica : IComandosAtribuicaoEsporadica
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;
        private readonly IServicoAtribuicaoEsporadica servicoAtribuicaoEsporadica;

        public ComandosAtribuicaoEsporadica(IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica, IServicoAtribuicaoEsporadica servicoAtribuicaoEsporadica)
        {
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.servicoAtribuicaoEsporadica = servicoAtribuicaoEsporadica ?? throw new System.ArgumentNullException(nameof(servicoAtribuicaoEsporadica));
        }

        public void Salvar(AtribuicaoEsporadicaCompletaDto atruibuicaoEsporadicaCompletaDto)
        {
            var entidade = ObterEntidade(atruibuicaoEsporadicaCompletaDto);

            servicoAtribuicaoEsporadica.Salvar(entidade, atruibuicaoEsporadicaCompletaDto.AnoLetivo);
        }

        private AtribuicaoEsporadica DtoParaEntidade(AtribuicaoEsporadicaCompletaDto Dto)
        {
            return new AtribuicaoEsporadica
            {
                UeId = Dto.UeId,
                DataFim = Dto.DataFim,
                DataInicio = Dto.DataInicio,
                DreId = Dto.DreId,
                Id = Dto.Id,
                ProfessorRf = Dto.ProfessorRf
            };
        }

        private AtribuicaoEsporadica ObterEntidade(AtribuicaoEsporadicaCompletaDto atribuicaoEsporadicaDto)
        {
            if (atribuicaoEsporadicaDto.Id == 0)
                return DtoParaEntidade(atribuicaoEsporadicaDto);

            var entidade = repositorioAtribuicaoEsporadica.ObterPorId(atribuicaoEsporadicaDto.Id);

            if (entidade == null || string.IsNullOrWhiteSpace(entidade.ProfessorRf))
                throw new NegocioException($"Não foi encontrado atribuição de codigo {atribuicaoEsporadicaDto.Id}");

            entidade.DataFim = atribuicaoEsporadicaDto.DataFim;
            entidade.DataInicio = atribuicaoEsporadicaDto.DataInicio;

            return entidade;
        }
    }
}