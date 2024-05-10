using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

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

        public async Task Atualizar(RegistroPoaDto registroPoaDto)
        {
            if (registroPoaDto.Id <= 0)
                throw new NegocioException("O id informado para edição tem que ser maior que 0");

            var entidade = repositorioRegistroPoa.ObterPorId(registroPoaDto.Id);

            if (entidade.EhNulo() || entidade.Excluido)
                throw new NegocioException("Registro para atualização não encontrado na base de dados");

            await MoverRemoverExcluidos(registroPoaDto, entidade);

            entidade = MapearParaAtualizacao(entidade, registroPoaDto);
            repositorioRegistroPoa.Salvar(entidade);
        }

        public async Task Cadastrar(RegistroPoaDto registroPoaDto)
        {
            await MoverRemoverExcluidos(registroPoaDto, new RegistroPoa() { Descricao = string.Empty });
            repositorioRegistroPoa.Salvar(MapearParaEntidade(registroPoaDto));
        }

        public async Task Excluir(long id)
        {
            if (id <= 0)
                throw new NegocioException("O id informado para edição tem que ser maior que 0");

            var entidadeBanco = repositorioRegistroPoa.ObterPorId(id);

            if (entidadeBanco.EhNulo() || entidadeBanco.Excluido)
                throw new NegocioException($"Não foi encontrado o registro de código {id}");

            entidadeBanco.Excluido = true;

            repositorioRegistroPoa.Salvar(entidadeBanco);
            if ((entidadeBanco?.Descricao).NaoEhNulo())
            {
                await mediator.Send(new DeletarArquivoDeRegistroExcluidoCommand(entidadeBanco.Descricao, TipoArquivo.RegistroPOA.Name()));
            }
        }

        private RegistroPoa MapearParaAtualizacao(RegistroPoa entidade, RegistroPoaDto registroPoaDto)
        {
            entidade.Titulo = registroPoaDto.Titulo;
            entidade.Descricao = registroPoaDto.Descricao;
            entidade.Bimestre = registroPoaDto.Bimestre;

            return entidade;
        }
        private async Task MoverRemoverExcluidos(RegistroPoaDto registroPoaDto, RegistroPoa entidade)
        {
            if (!string.IsNullOrEmpty(registroPoaDto.Descricao))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.RegistroPOA, entidade.Descricao, registroPoaDto.Descricao));
                registroPoaDto.Descricao = moverArquivo;
            }
            if (!string.IsNullOrEmpty(entidade.Descricao))
            {
                var deletarArquivosNaoUtilziados = await mediator.Send(new RemoverArquivosExcluidosCommand(entidade.Descricao, registroPoaDto.Descricao, TipoArquivo.RegistroPOA.Name()));
            }
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
