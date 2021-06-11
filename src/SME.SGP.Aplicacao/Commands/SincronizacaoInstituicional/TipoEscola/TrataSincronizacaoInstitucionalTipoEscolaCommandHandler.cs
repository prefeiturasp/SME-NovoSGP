using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTipoEscolaCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalTipoEscolaCommand, bool>
    {
        private readonly IRepositorioTipoEscola repositorioTipoEscola;

        public TrataSincronizacaoInstitucionalTipoEscolaCommandHandler(IRepositorioTipoEscola repositorioTipoEscola)
        {
            this.repositorioTipoEscola = repositorioTipoEscola ?? throw new System.ArgumentNullException(nameof(repositorioTipoEscola));
        }
        public async Task<bool> Handle(TrataSincronizacaoInstitucionalTipoEscolaCommand request, CancellationToken cancellationToken)
        {

            if (request.TipoEscolaSGP == null)
            {
                var tipoEscolaParaAdicionar = new TipoEscolaEol()
                {
                    CodEol = request.TipoEscolaEol.Codigo,
                    Descricao = request.TipoEscolaEol.DescricaoSigla,
                    DtAtualizacao = request.TipoEscolaEol.DtAtualizacao
                };

                await repositorioTipoEscola.SalvarAsync(tipoEscolaParaAdicionar);
            }
            else
            {
                if (request.TipoEscolaSGP.Descricao != request.TipoEscolaEol.DescricaoSigla)
                {
                    request.TipoEscolaSGP.Descricao = request.TipoEscolaEol.DescricaoSigla;
                    request.TipoEscolaSGP.DtAtualizacao = request.TipoEscolaEol.DtAtualizacao;
                    await repositorioTipoEscola.SalvarAsync(request.TipoEscolaSGP);
                }
            }


            return true;

        }
    }
}
