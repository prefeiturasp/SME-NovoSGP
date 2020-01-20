using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ComandosRegistroPoa : IComandosRegistroPoa
    {
        private readonly IRepositorioRegistroPoa repositorioRegistroPoa;

        public ComandosRegistroPoa(IRepositorioRegistroPoa repositorioRegistroPoa)
        {
            this.repositorioRegistroPoa = repositorioRegistroPoa ?? throw new System.ArgumentNullException(nameof(repositorioRegistroPoa));
        }

        public void Atualizar(RegistroPoaDto registroPoaDto)
        {
            if (registroPoaDto.Id <= 0)
                throw new NegocioException("O id informado para edição tem que ser maior que 0");

            repositorioRegistroPoa.Salvar(MapearParaAtualizacao(registroPoaDto));
        }

        public void Cadastrar(RegistroPoaDto registroPoaDto)
        {
            repositorioRegistroPoa.Salvar(MapearParaEntidade(registroPoaDto));
        }

        public void Excluir(long id)
        {
            if (id <= 0)
                throw new NegocioException("O id informado para edição tem que ser maior que 0");

            var entidadeBanco = repositorioRegistroPoa.ObterPorId(id);

            if (entidadeBanco == null || entidadeBanco.Excluido)
                throw new NegocioException($"Não foi encontrado o registro de código {id}");

            entidadeBanco.Excluido = true;

            repositorioRegistroPoa.Salvar(entidadeBanco);
        }

        private RegistroPoa MapearParaAtualizacao(RegistroPoaDto registroPoaDto)
        {
            var entidade = repositorioRegistroPoa.ObterPorId(registroPoaDto.Id);

            if (entidade == null || entidade.Excluido)
                throw new NegocioException("Registro para atualização não encontrado na base de dados");

            entidade.Titulo = registroPoaDto.Titulo;
            entidade.Descricao = registroPoaDto.Descricao;
            entidade.Bimestre = registroPoaDto.Bimestre;

            return entidade;
        }

        private RegistroPoa MapearParaEntidade(RegistroPoaDto registroPoaDto)
        {
            return new RegistroPoa
            {
                Descricao = registroPoaDto.Descricao,
                DreId = registroPoaDto.DreId,
                AnoLetivo = registroPoaDto.AnoLetivo,
                UeId = registroPoaDto.UeId,
                CodigoRf = registroPoaDto.CodigoRf,
                Bimestre = registroPoaDto.Bimestre,
                Titulo = registroPoaDto.Titulo,
            };
        }
    }
}
