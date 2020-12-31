using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosIndividuaisPorAlunoPeriodoQueryHandler : IRequestHandler<ObterRegistrosIndividuaisPorAlunoPeriodoQuery, RegistrosIndividuaisPeriodoDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ObterRegistrosIndividuaisPorAlunoPeriodoQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual, IMediator mediator)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new System.ArgumentNullException(nameof(repositorioRegistroIndividual));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<RegistrosIndividuaisPeriodoDto> Handle(ObterRegistrosIndividuaisPorAlunoPeriodoQuery request, CancellationToken cancellationToken)
        {
            var registrosIndividuais = await repositorioRegistroIndividual.ObterPorAlunoPeriodo(request.TurmaId, request.ComponenteCurricularId, request.AlunoCodigo, request.DataInicio, request.DataFim);

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));

            return new RegistrosIndividuaisPeriodoDto()
            {
                PodeRealizarNovoRegistro = !turma.EhTurmaHistorica,
                RegistrosIndividuais = MapearParaDto(registrosIndividuais)
            };
        }

        private IEnumerable<RegistroIndividualDto> MapearParaDto(IEnumerable<RegistroIndividual> registros)
        {
            if (registros != null && registros.Any())
            {
                foreach (var registro in registros)
                {
                    yield return new RegistroIndividualDto()
                    {
                        AlunoCodigo = registro.AlunoCodigo,
                        Auditoria = (AuditoriaDto)registro,
                        ComponenteCurricularId = registro.ComponenteCurricularId,
                        Data = registro.DataRegistro,
                        Excluido = registro.Excluido,
                        Migrado = registro.Migrado,
                        Registro = registro.Registro,
                        TurmaId = registro.TurmaId
                    };
                }
            }
            else
               yield return null;
        }
    }
}
