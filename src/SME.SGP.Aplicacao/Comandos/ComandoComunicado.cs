using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandoComunicado : IComandoComunicado
    {
        private readonly IRepositorioComunicado repositorio;
        private readonly IRepositorioComunicadoGrupo repositorioComunicadoGrupo;
        private readonly IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar;
        private readonly IUnitOfWork unitOfWork;

        public ComandoComunicado(IRepositorioComunicado repositorio,
            IServicoAcompanhamentoEscolar servicoAcompanhamentoEscolar,
            IRepositorioComunicadoGrupo repositorioComunicadoGrupo,
            IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
            this.repositorioComunicadoGrupo = repositorioComunicadoGrupo ?? throw new System.ArgumentNullException(nameof(repositorioComunicadoGrupo));
            this.servicoAcompanhamentoEscolar = servicoAcompanhamentoEscolar ?? throw new System.ArgumentNullException(nameof(servicoAcompanhamentoEscolar));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<string> Alterar(long id, ComunicadoInserirDto comunicadoDto)
        {
            Comunicado comunicado = BuscarComunicado(id);
            MapearParaEntidade(comunicadoDto, comunicado);

            try
            {
                unitOfWork.IniciarTransacao();
                await repositorioComunicadoGrupo.ExcluirPorIdComunicado(id);
                await SalvarGrupos(id, comunicadoDto);
                await repositorio.SalvarAsync(comunicado);
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return "Comunicado alterado com sucesso";
        }

        public async Task<string> Excluir(long id)
        {
            var comunicado = BuscarComunicado(id);

            try
            {
                unitOfWork.IniciarTransacao();
                await repositorioComunicadoGrupo.ExcluirPorIdComunicado(id);
                comunicado.MarcarExcluido();
                await repositorio.SalvarAsync(comunicado);
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return "Comunicado excluído com sucesso";
        }

        public async Task<string> Inserir(ComunicadoInserirDto comunicadoDto)
        {
            Comunicado comunicado = new Comunicado();
            MapearParaEntidade(comunicadoDto, comunicado);
            try
            {
                unitOfWork.IniciarTransacao();

                //await servicoAcompanhamentoEscolar.CriarComunicado(comunicadoDto);

                var id = await repositorio.SalvarAsync(comunicado);
                await SalvarGrupos(id, comunicadoDto);
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return "Comunicado criado com sucesso";
        }

        private static void MapearParaEntidade(ComunicadoInserirDto comunicadoDto, Comunicado comunicado)
        {
            comunicado.DataEnvio = comunicadoDto.DataEnvio;
            comunicado.DataExpiracao = comunicadoDto.DataExpiracao;
            comunicado.Descricao = comunicadoDto.Descricao;
            comunicado.Titulo = comunicadoDto.Titulo;
        }

        private Comunicado BuscarComunicado(long id)
        {
            var comunicado = repositorio.ObterPorId(id);
            if (comunicado is null)
                throw new NegocioException("Comunicado não encontrado");
            return comunicado;
        }

        private async Task SalvarGrupos(long id, ComunicadoInserirDto comunicadoDto)
        {
            foreach (var grupoId in comunicadoDto.GruposId)
            {
                await repositorioComunicadoGrupo.SalvarAsync(new ComunicadoGrupo { ComunicadoId = id, GrupoComunicadoId = grupoId });
            }
        }
    }
}