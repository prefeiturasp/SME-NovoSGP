using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

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


        public async Task Salvar(AtribuicaoEsporadicaDto atruibuicaoEsporadicaDto)
        {
            var entidade = ObterEntidade(atruibuicaoEsporadicaDto);

            await servicoAtribuicaoEsporadica.Salvar(entidade, atruibuicaoEsporadicaDto.AnoLetivo, atruibuicaoEsporadicaDto.EhInfantil);
        }

        private AtribuicaoEsporadica DtoParaEntidade(AtribuicaoEsporadicaDto Dto)
        {
            return new AtribuicaoEsporadica
            {
                UeId = Dto.UeId,
                DataFim = Dto.DataFim.Local().Date,
                DataInicio = Dto.DataInicio.Local().Date,
                DreId = Dto.DreId,
                Id = Dto.Id,
                ProfessorRf = Dto.ProfessorRf,
                AnoLetivo = Dto.AnoLetivo
            };
        }

        private AtribuicaoEsporadica ObterEntidade(AtribuicaoEsporadicaDto atribuicaoEsporadicaDto)
        {
            if (atribuicaoEsporadicaDto.Id == 0)
                return DtoParaEntidade(atribuicaoEsporadicaDto);

            var entidade = repositorioAtribuicaoEsporadica.ObterPorId(atribuicaoEsporadicaDto.Id);

            if (entidade == null || string.IsNullOrWhiteSpace(entidade.ProfessorRf))
                throw new NegocioException($"Não foi encontrado atribuição de codigo {atribuicaoEsporadicaDto.Id}");

            entidade.DataFim = atribuicaoEsporadicaDto.DataFim.Local();
            entidade.DataInicio = atribuicaoEsporadicaDto.DataInicio.Local();
            entidade.AnoLetivo = atribuicaoEsporadicaDto.AnoLetivo;

            return entidade;
        }
    }
}