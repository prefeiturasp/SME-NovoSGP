using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesUseCase : AbstractUseCase, ISalvarInformesUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioCorrelacaoInforme repositorioCorrelacaoInforme;
        public SalvarInformesUseCase(IMediator mediator, IUnitOfWork unitOfWork, IRepositorioCorrelacaoInforme _repositorioCorrelacaoInforme) : base(mediator)
        {
            this.unitOfWork = unitOfWork;
            repositorioCorrelacaoInforme = _repositorioCorrelacaoInforme;
        }

        public async Task<AuditoriaDto> Executar(InformesDto informesDto)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            unitOfWork.IniciarTransacao();
            try
            {
                var informes = await mediator.Send(new SalvarInformesCommand(informesDto));

                foreach (var perfil in informesDto.Perfis) 
                    await mediator.Send(new SalvarInformesPerfilsCommand(informes.Id, perfil.Id));

                await CadastrarModalidades(informes.Id, informesDto);

                if (informesDto.Arquivos.PossuiRegistros())
                    await mediator.Send(new SalvarInformesAnexosCommand(informes.Id, informesDto.Arquivos));

                var correlacao = new InformeCorrelacao(informes.Id, usuarioLogado.Id);
                repositorioCorrelacaoInforme.Salvar(correlacao);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoInformativo, informes.Id.ToString(), correlacao.Codigo, usuarioLogado, false));

                unitOfWork.PersistirTransacao();

                return ObterAuditoria(informes);
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task CadastrarModalidades(long id, InformesDto informesDto)
        {
            if (informesDto.Modalidades == null)
                return;

            foreach (var modalidade in informesDto?.Modalidades)
                await mediator.Send(new SalvarInformesModalidadeCommand(id, modalidade));
        }

        private AuditoriaDto ObterAuditoria(Informativo informativo)
        {
            return new AuditoriaDto()
            {
                Id = informativo.Id,
                CriadoEm = informativo.CriadoEm,
                CriadoPor = informativo.CriadoPor,
                CriadoRF = informativo.CriadoRF,
                AlteradoEm = informativo.AlteradoEm,
                AlteradoPor = informativo.AlteradoPor,
                AlteradoRF = informativo.AlteradoRF
            };
        }
    }
}
