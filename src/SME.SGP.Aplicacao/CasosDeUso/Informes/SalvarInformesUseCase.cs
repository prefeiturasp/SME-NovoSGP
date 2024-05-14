using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesUseCase : AbstractUseCase, ISalvarInformesUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public SalvarInformesUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<AuditoriaDto> Executar(InformesDto informesDto)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                var informes = await mediator.Send(new SalvarInformesCommand(informesDto));

                foreach (var perfil in informesDto.Perfis) 
                    await mediator.Send(new SalvarInformesPerfilsCommand(informes.Id, perfil.Id));
                if (informesDto.Arquivos.Any())
                    await mediator.Send(new SalvarInformesAnexosCommand(informes.Id, informesDto.Arquivos));
                
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoInformativo, informes.Id, Guid.NewGuid()));
                unitOfWork.PersistirTransacao();
                return ObterAuditoria(informes);
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
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
