using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEETurmaAlunoPorIdQueryHandler : IRequestHandler<ObterEncaminhamentoAEETurmaAlunoPorIdQuery, IEnumerable<EncaminhamentoAEETurmaAluno>>
    {
        private readonly IRepositorioEncaminhamentoAEETurmaAlunoConsulta repositorioEncaminhamentoAEETurmaAlunoConsulta;

        public ObterEncaminhamentoAEETurmaAlunoPorIdQueryHandler(IRepositorioEncaminhamentoAEETurmaAlunoConsulta repositorioEncaminhamentoAEETurmaAlunoConsulta)
        {
            this.repositorioEncaminhamentoAEETurmaAlunoConsulta = repositorioEncaminhamentoAEETurmaAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEETurmaAlunoConsulta));
        }

        public Task<IEnumerable<EncaminhamentoAEETurmaAluno>> Handle(ObterEncaminhamentoAEETurmaAlunoPorIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioEncaminhamentoAEETurmaAlunoConsulta.ObterEncaminhamentoAEETurmaAlunoPorIdAsync(request.EncaminhamentoAEEId);
        }
    }
}
