using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase : IListarObjetivoAprendizagemPorAnoTurmaEComponenteCurricularUseCase
    {
        private readonly IMediator mediator;

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Executar(string ano, long componenteCurricularId, bool ensinoEspecial, long turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));
            
            if (new[] { Modalidade.EJA, Modalidade.Medio }.Contains(turma.ModalidadeCodigo))
                return null;

            long[] ids = componenteCurricularId == 138 ?
                new long[] { (ensinoEspecial ? 11 : 6) } :
                await mediator.Send(new ObterJuremaIdsPorComponentesCurricularIdQuery(componenteCurricularId));

            var anos = new string[] { ano };
            if (ensinoEspecial)
                anos = Enumerable.Range(1, 9).Select(a => a.ToString()).ToArray();
            else if (turma.EhTurmaPrograma())
                anos = Array.Empty<string>();

            var objetivos = await mediator.Send(
                new ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(
                    anos,
                    ids));

            foreach (var item in objetivos)
                item.ComponenteCurricularEolId = componenteCurricularId;

            if ((ensinoEspecial || turma.EhTurmaPrograma()) && !anos.Contains(ano))
                return objetivos.OrderBy(o => Enum.Parse(typeof(AnoTurma), o.Ano)).ThenBy(x => x.Codigo);

            if (int.TryParse(ano, out int anoTurma))
                objetivos = objetivos.Where(x => x.Ano == ((AnoTurma)anoTurma).Name()).ToList();

            return objetivos.OrderBy(o => o.Codigo);
        }
    }
}
