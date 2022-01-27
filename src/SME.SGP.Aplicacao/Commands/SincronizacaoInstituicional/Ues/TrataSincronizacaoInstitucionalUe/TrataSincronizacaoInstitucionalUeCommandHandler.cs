using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalUeCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalUeCommand, bool>
    {
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioDreConsulta repositorioDre;

        public TrataSincronizacaoInstitucionalUeCommandHandler(IRepositorioUe repositorioUe, IRepositorioDreConsulta repositorioDre)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<bool> Handle(TrataSincronizacaoInstitucionalUeCommand request, CancellationToken cancellationToken)
        {

            if (request.UeSGP == null)
            {
                var dreDaUe = repositorioDre.ObterPorCodigo(request.UeEOL.DreCodigo.ToString());

                if (dreDaUe == null)
                    throw new NegocioException($"Não foi possível localizar a Dre da Ue {request.UeEOL.UeCodigo}");

                var ueParaIncluir = new Ue()
                {
                    CodigoUe = request.UeEOL.UeCodigo,
                    Nome = request.UeEOL.UeNome.Trim(),
                    TipoEscola = (TipoEscola)request.UeEOL.TipoEscolaCodigo
                };

                ueParaIncluir.AdicionarDre(dreDaUe);

                var idNovaUe = await repositorioUe.IncluirAsync(ueParaIncluir);
                if (idNovaUe <= 0)
                    throw new NegocioException($"Erro ao incluir nova Ue {request.UeEOL.UeCodigo}");
                
            }
            else
            {
                if (VerificaSeTemAlteracao(request.UeEOL, request.UeSGP))
                {
                    var ueParaAtualizar = new Ue()
                    {
                        CodigoUe = request.UeEOL.UeCodigo,
                        Nome = request.UeEOL.UeNome.Trim(),
                        TipoEscola = (TipoEscola)request.UeEOL.TipoEscolaCodigo,
                        Id = request.UeSGP.Id,
                        DreId = request.UeSGP.DreId
                    };

                    await repositorioUe.AtualizarAsync(ueParaAtualizar);
                }
            }

            return true;
        }

        private bool VerificaSeTemAlteracao(UeDetalhesParaSincronizacaoInstituicionalDto ueEOL, Ue ueSGP)
        {
            if (ueEOL.UeNome.Trim() != ueSGP.Nome.Trim() ||
                ueEOL.TipoEscolaCodigo != (int)ueSGP.TipoEscola)
                return true;

            return false;
        }
    }
}
