using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarAcompanhamentoAlunoFotoCommandHandler : IRequestHandler<GerarAcompanhamentoAlunoFotoCommand, long>
    {
        private readonly IRepositorioAcompanhamentoAlunoFoto repositorio;

        public GerarAcompanhamentoAlunoFotoCommandHandler(IRepositorioAcompanhamentoAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(GerarAcompanhamentoAlunoFotoCommand request, CancellationToken cancellationToken)
            => await repositorio.SalvarAsync(new Dominio.AcompanhamentoAlunoFoto()
            {
                AcompanhamentoAlunoSemestreId = request.AcompanhamentoAlunoSemestreId,
                ArquivoId = request.ArquivoId,
                MiniaturaId = request.MiniaturaId
            });
    }
}
