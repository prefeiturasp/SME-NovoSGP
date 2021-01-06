using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosIndividuaisPorAlunoPeriodoQueryHandler : ConsultasBase, IRequestHandler<ObterRegistrosIndividuaisPorAlunoPeriodoQuery, RegistrosIndividuaisPeriodoDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ObterRegistrosIndividuaisPorAlunoPeriodoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRegistroIndividual repositorioRegistroIndividual, IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new System.ArgumentNullException(nameof(repositorioRegistroIndividual));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<RegistrosIndividuaisPeriodoDto> Handle(ObterRegistrosIndividuaisPorAlunoPeriodoQuery request, CancellationToken cancellationToken)
        {
            var registrosIndividuais = await repositorioRegistroIndividual.ObterPorAlunoPeriodoPaginado(request.TurmaId, request.ComponenteCurricularId, request.AlunoCodigo, request.DataInicio, request.DataFim, Paginacao);

            PaginacaoResultadoDto<RegistroIndividualDto> registrosDto = null;

            if (registrosIndividuais != null && registrosIndividuais.Items.Any())
                registrosDto = MapearParaDto(registrosIndividuais);

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));

            return new RegistrosIndividuaisPeriodoDto()
            {
                PodeRealizarNovoRegistro = !turma.EhTurmaHistorica,
                RegistrosIndividuais = registrosDto
            };
        }

        private PaginacaoResultadoDto<RegistroIndividualDto> MapearParaDto(PaginacaoResultadoDto<RegistroIndividual> registros)
        {
            return new PaginacaoResultadoDto<RegistroIndividualDto>()
            {
                Items = registros.Items.Select(registro =>
                {
                    return new RegistroIndividualDto()
                    {
                        Id = registro.Id,
                        AlunoCodigo = registro.AlunoCodigo,
                        Auditoria = (AuditoriaDto)registro,
                        ComponenteCurricularId = registro.ComponenteCurricularId,
                        Data = registro.DataRegistro,
                        Excluido = registro.Excluido,
                        Migrado = registro.Migrado,
                        Registro = registro.Registro,
                        TurmaId = registro.TurmaId
                    };
                }),
                TotalPaginas = registros.TotalPaginas,
                TotalRegistros = registros.TotalRegistros
            };
        }
    }
}
