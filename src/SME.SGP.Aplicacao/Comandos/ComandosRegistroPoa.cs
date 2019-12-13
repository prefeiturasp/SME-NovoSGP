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
            this.repositorioRegistroPoa = repositorioRegistroPoa;
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

        public RegistroPoa MapearParaAtualizacao(RegistroPoaDto registroPoaDto)
        {
            var entidade = repositorioRegistroPoa.ObterPorId(registroPoaDto.Id);

            if (entidade == null)
                throw new NegocioException("Registro para atualização não encontrado na base de dados");

            entidade.Titulo = registroPoaDto.Titulo;
            entidade.Descricao = registroPoaDto.Descricao;
            entidade.Mes = registroPoaDto.Mes;

            return entidade;
        }

        public RegistroPoa MapearParaEntidade(RegistroPoaDto registroPoaDto)
        {
            return new RegistroPoa
            {
                Descricao = registroPoaDto.Descricao,
                DreId = registroPoaDto.DreId,
                UeId = registroPoaDto.UeId,
                CodigoRf = registroPoaDto.CodigoRf,
                Mes = registroPoaDto.Mes,
                Titulo = registroPoaDto.Titulo,
            };
        }
    }
}