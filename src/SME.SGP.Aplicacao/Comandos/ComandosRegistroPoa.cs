using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ComandosRegistroPoa : IComandosRegistroPoa
    {
        private readonly IRepositorioRegistroPoa repositorioRegistroPoa;
        private readonly IMediator mediator;

        public ComandosRegistroPoa(IRepositorioRegistroPoa repositorioRegistroPoa, IMediator mediator)
        {
            this.repositorioRegistroPoa = repositorioRegistroPoa ?? throw new System.ArgumentNullException(nameof(repositorioRegistroPoa));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
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
            MoverRemoverExcluidos(registroPoaDto, entidade);
            entidade.Titulo = registroPoaDto.Titulo;
            entidade.Descricao = registroPoaDto.Descricao;
            entidade.Bimestre = registroPoaDto.Bimestre;

            return entidade;
        }
        private void MoverRemoverExcluidos(RegistroPoaDto registroPoaDto, RegistroPoa entidade)
        {
            if (!string.IsNullOrEmpty(registroPoaDto.Descricao))
            {
                var moverArquivo = mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.RegistroPOA, entidade.Descricao, registroPoaDto.Descricao));
                registroPoaDto.Descricao = moverArquivo.Result;
            }
            if (!string.IsNullOrEmpty(entidade.Descricao))
            {
                var deletarArquivosNaoUtilziados = mediator.Send(new RemoverArquivosExcluidosCommand(entidade.Descricao, registroPoaDto.Descricao, TipoArquivo.RegistroPOA.Name()));
            }
        }
        private RegistroPoa MapearParaEntidade(RegistroPoaDto registroPoaDto)
        {
            MoverRemoverExcluidos(registroPoaDto, new RegistroPoa() {Descricao = string.Empty });
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
