using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroColetivoCommandHandler : IRequestHandler<InserirRegistroColetivoCommand, ResultadoRegistroColetivoDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroColetivo repositorio;
        private readonly IUnitOfWork unitOfWork;

        public InserirRegistroColetivoCommandHandler(IMediator mediator,
                                                     IRepositorioRegistroColetivo repositorio,
                                                     IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ResultadoRegistroColetivoDto> Handle(InserirRegistroColetivoCommand request, CancellationToken cancellationToken)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                var registro = ObterRegistro(request.RegistroColetivo);
                await repositorio.SalvarAsync(registro);

                await mediator.Send(new InserirRegistroColetivoUeCommand(registro.Id, request.RegistroColetivo.UeIds));

                if (request.RegistroColetivo.Anexos.NaoEhNulo() &&
                    request.RegistroColetivo.Anexos.Any())
                    await mediator.Send(new InserirRegistroColetivoAnexoCommand(registro.Id, request.RegistroColetivo.Anexos));

                unitOfWork.PersistirTransacao();

                return new ResultadoRegistroColetivoDto()
                {
                    Id = registro.Id,
                    Auditoria = new AuditoriaDto()
                    {
                        CriadoPor = registro.CriadoPor,
                        CriadoRF = registro.CriadoRF
                    }
                };
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
            }

            return null;
        }

        private RegistroColetivo ObterRegistro(RegistroColetivoDto registroColetivoDto)
        {
            return new RegistroColetivo()
            {
                DreId = registroColetivoDto.DreId,
                TipoReuniaoId = registroColetivoDto.TipoReuniaoId,
                DataRegistro = registroColetivoDto.DataRegistro,
                QuantidadeCuidadores = registroColetivoDto.QuantidadeCuidadores,
                QuantidadeEducadores = registroColetivoDto.QuantidadeEducadores,
                QuantidadeEducandos = registroColetivoDto.QuantidadeEducandos,
                QuantidadeParticipantes = registroColetivoDto.QuantidadeParticipantes,
                Descricao = registroColetivoDto.Descricao,
                Observacao = registroColetivoDto.Observacao,
            };
        }
    }
}
