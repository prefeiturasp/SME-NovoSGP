using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosBuscaAtivaPorAlunoQueryHandler : IRequestHandler<ObterDadosBuscaAtivaPorAlunoQuery, DadosBuscaAtivaAlunoDto>
    {
        private readonly IRepositorioRegistroAcaoBuscaAtiva repositorio;

        public ObterDadosBuscaAtivaPorAlunoQueryHandler(IRepositorioRegistroAcaoBuscaAtiva repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<DadosBuscaAtivaAlunoDto> Handle(ObterDadosBuscaAtivaPorAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterDadosBuscaAtivaPorAluno(request.AlunoCodigo, request.AnoLetivo);
        }
    }
}