using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarInformesUseCase : AbstractUseCase, ISalvarInformesUseCase
    {
        public SalvarInformesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InformesDto informesDto)
        {
            var informes = await mediator.Send(new SalvarInformesCommand(informesDto));

            foreach (var perfil in informesDto.Perfis) 
            {
                await mediator.Send(new SalvarInformesPerfilsCommand(informes.Id, perfil.Id));
            }

            return ObterAuditoria(informes);
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
