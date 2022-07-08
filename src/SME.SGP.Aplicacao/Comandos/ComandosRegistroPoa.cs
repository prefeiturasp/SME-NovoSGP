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

            repositorioRegistroPoa.Salvar(await MapearParaAtualizacao(registroPoaDto));
        }

        public async Task Cadastrar(RegistroPoaDto registroPoaDto)
        {
            repositorioRegistroPoa.Salvar(await MapearParaEntidade(registroPoaDto));
        }

        public async Task Excluir(long id)
        {
            if (id <= 0)
                throw new NegocioException("O id informado para edição tem que ser maior que 0");

            var entidadeBanco = await repositorioRegistroPoa.ObterPorIdAsync(id);

            if (entidadeBanco == null || entidadeBanco.Excluido)
                throw new NegocioException($"Não foi encontrado o registro de código {id}");

            entidadeBanco.Excluido = true;

            await repositorioRegistroPoa.SalvarAsync(entidadeBanco);
            if (entidadeBanco?.Descricao != null)
            {
                await mediator.Send(new DeletarArquivoDeRegistroExcluidoCommand(entidadeBanco.Descricao, TipoArquivo.RegistroPOA.Name()));
            }
        }

        private async Task<RegistroPoa> MapearParaAtualizacao(RegistroPoaDto registroPoaDto)
        {
            var entidade = repositorioRegistroPoa.ObterPorId(registroPoaDto.Id);

            if (entidade == null || entidade.Excluido)
                throw new NegocioException("Registro para atualização não encontrado na base de dados");
            await MoverRemoverExcluidos(registroPoaDto, entidade);
            entidade.Titulo = registroPoaDto.Titulo;
            entidade.Descricao = registroPoaDto.Descricao;
            entidade.Bimestre = registroPoaDto.Bimestre;

            return entidade;
        }
        private async Task MoverRemoverExcluidos(RegistroPoaDto registroPoaDto, RegistroPoa entidade)
        {
            if (!string.IsNullOrEmpty(registroPoaDto.Descricao))
            {
                registroPoaDto.Descricao = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.RegistroPOA, entidade.Descricao, registroPoaDto.Descricao));
            }
            if (!string.IsNullOrEmpty(entidade.Descricao))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(entidade.Descricao, registroPoaDto.Descricao, TipoArquivo.RegistroPOA.Name()));
            }
        }
        private async Task<RegistroPoa> MapearParaEntidade(RegistroPoaDto registroPoaDto)
        {
            await MoverRemoverExcluidos(registroPoaDto, new RegistroPoa() {Descricao = string.Empty });
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
