using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosAnoAtualQueryHandler : IRequestHandler<ObterComunicadosAnoAtualQuery, IEnumerable<ComunicadoTurmaAlunoDto>>
    {
        private readonly IRepositorioComunicadoConsulta repositorioComunicado;

        public ObterComunicadosAnoAtualQueryHandler(IRepositorioComunicadoConsulta repositorioComunicado)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
        }

        public Task<IEnumerable<ComunicadoTurmaAlunoDto>> Handle(ObterComunicadosAnoAtualQuery request, CancellationToken cancellationToken)
        {
            return repositorioComunicado.ObterComunicadosAnoAtual();
        }
    }
}