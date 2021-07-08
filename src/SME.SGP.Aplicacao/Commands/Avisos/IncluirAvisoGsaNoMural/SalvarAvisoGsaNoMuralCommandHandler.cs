using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAvisoGsaNoMuralCommandHandler : AsyncRequestHandler<SalvarAvisoGsaNoMuralCommand>
    {
        private readonly IRepositorioAviso repositorioAviso;

        public SalvarAvisoGsaNoMuralCommandHandler(IRepositorioAviso repositorioAviso)
        {
            this.repositorioAviso = repositorioAviso ?? throw new ArgumentNullException(nameof(repositorioAviso));
        }

        protected override async Task Handle(SalvarAvisoGsaNoMuralCommand request, CancellationToken cancellationToken)
        {
            var aviso = await repositorioAviso.ObterPorClassroomId(request.AvisoClassroomId);

            MapearParaEntidade(request, aviso);

            await repositorioAviso.SalvarAsync(aviso);
        }

        private void MapearParaEntidade(SalvarAvisoGsaNoMuralCommand request, Dominio.Aviso aviso)
        {
            if (aviso == null)
                aviso = new Dominio.Aviso();

            aviso.AulaId = request.AulaId;
            aviso.Mensagem = request.Mensagem;
            aviso.AvisoClassroomId = request.AvisoClassroomId;
            aviso.CriadoRF = request.UsuarioRf;
            aviso.CriadoEm = request.DataCriacao;
            aviso.AlteradoRF = request.DataAlteracao.HasValue ? request.UsuarioRf : String.Empty;
            aviso.AlteradoEm = request.DataAlteracao;
        }
    }
}
