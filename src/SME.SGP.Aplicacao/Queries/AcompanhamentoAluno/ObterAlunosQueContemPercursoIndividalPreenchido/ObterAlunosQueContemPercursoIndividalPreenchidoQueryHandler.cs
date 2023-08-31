using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosQueContemPercursoIndividalPreenchidoQueryHandler : IRequestHandler<ObterAlunosQueContemPercursoIndividalPreenchidoQuery, IEnumerable<AcompanhamentoAluno>>
    {
        private readonly IRepositorioAcompanhamentoAlunoConsulta repositorio;

        public ObterAlunosQueContemPercursoIndividalPreenchidoQueryHandler(IRepositorioAcompanhamentoAlunoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<AcompanhamentoAluno>> Handle(ObterAlunosQueContemPercursoIndividalPreenchidoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAlunosQueContemPercursoIndividalPreenchido(request.TurmaId, request.Semestre);
        }
    }
}
