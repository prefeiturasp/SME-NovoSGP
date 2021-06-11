using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarFotoEstudanteCommandHandler : IRequestHandler<GerarFotoEstudanteCommand, long>
    {
        private readonly IRepositorioAlunoFoto repositorio;

        public GerarFotoEstudanteCommandHandler(IRepositorioAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(GerarFotoEstudanteCommand request, CancellationToken cancellationToken)
            => await repositorio.SalvarAsync(new AlunoFoto()
            {
                MiniaturaId = request.MiniaturaId,
                AlunoCodigo = request.AlunoCodigo,
                ArquivoId = request.ArquivoId
            });
    }
}
