using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaDresUseCase : AbstractUseCase, IObterAbrangenciaDresUseCase
    {
        public ObterAbrangenciaDresUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> Executar(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string filtro = "")
        {
            var login = await mediator.Send(new ObterLoginAtualQuery());
            var perfil = await mediator.Send(new ObterPerfilAtualQuery());
            var filtroEhCodigo = false;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                if (filtro.All(char.IsDigit))
                    filtroEhCodigo = true;
            }

            return await mediator.Send(new ObterAbrangenciaDresQuery(login, perfil, modalidade, periodo, consideraHistorico, anoLetivo, filtro, filtroEhCodigo));
        }
    }
}