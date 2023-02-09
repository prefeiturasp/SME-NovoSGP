﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PodeCadastrarAulaUseCase : AbstractUseCase, IPodeCadastrarAulaUseCase
    {
        public PodeCadastrarAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<CadastroAulaDto> Executar(FiltroPodeCadastrarAulaDto filtro)
        {
            if (CriandoAula(filtro.AulaId) || await AlterandoDataAula(filtro.AulaId, filtro.DataAula))
            {
                var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
                if (!await mediator.Send(new PodeCadastrarAulaNoDiaQuery(filtro.DataAula, filtro.TurmaCodigo, filtro.ComponenteCurricular, usuarioLogado.CodigoRf, filtro.TipoAula)))
                    throw new NegocioException($"Não é possível cadastrar aula do tipo '{filtro.TipoAula.Name()}' para o dia selecionado!");
            }

            return new CadastroAulaDto()
            {
                PodeCadastrarAula = true,
                Grade = filtro.TipoAula == TipoAula.Reposicao ? null : await mediator.Send(new ObterGradeAulasPorTurmaEProfessorQuery(filtro.TurmaCodigo, filtro.ComponenteCurricular, filtro.DataAula, ehRegencia: filtro.EhRegencia))
            };
        }

        private async Task<bool> AlterandoDataAula(long aulaId, DateTime dataAula)
        {
            var dataOriginalAula = await mediator.Send(new ObterDataAulaQuery(aulaId));
            return dataAula != dataOriginalAula;
        }

        private static bool CriandoAula(long aulaId)
            => aulaId == 0;
    }
}
