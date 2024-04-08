using MediatR;
using Org.BouncyCastle.Asn1;
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
        private const int ANO_TURMA_EJA_INICIA_A_PARTIR_DE = 9;
        private readonly string[] ANOS_OBJETIVO_APRENDIZAGEM_ENSINO_ESPECIAL = Enumerable.Range(1, 9).Select(a => a.ToString()).ToArray();
        private readonly string[] ANOS_OBJETIVO_APRENDIZAGEM_EJA = Enumerable.Range(10, 4).Select(a => a.ToString()).ToArray();

        

        public ListarObjetivoAprendizagemPorAnoEComponenteCurricularUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Executar(string ano, long componenteCurricularId, bool ensinoEspecial, long turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));
            if (!turma.ModalidadeCodigo.PossuiObjetivosAprendizagem())
                return null;

            long[] ids = await ObterIdsJurema(componenteCurricularId, ensinoEspecial);
            var anos = ObterAnosFiltroObjetivosAprendizagem(turma, ensinoEspecial, new string[] { ano });
            var objetivos = (await mediator.Send(
                             new ListarObjetivoAprendizagemPorAnoEComponenteCurricularQuery(anos, ids)))
                             .ToList();
            objetivos.ForEach(obj => obj.ComponenteCurricularEolId = componenteCurricularId);

            if ((ensinoEspecial || turma.EhTurmaPrograma()) && !anos.Contains(ano))
                return objetivos.OrderBy(o => Enum.Parse(typeof(AnoTurma), o.Ano)).ThenBy(x => x.Codigo);

            if (int.TryParse(ano, out int anoTurma))
                objetivos = objetivos.Where(x => x.Ano == ObterAnoTurmaObjetivosAprendizagem(anoTurma, turma)).ToList();

            return objetivos.OrderBy(o => o.Codigo);
        }

        private string ObterAnoTurmaObjetivosAprendizagem(int anoTurma, Turma turma)
        {
            var anoTurmaObjetivosAprendizagem = 0;
            if (turma.EhEJA())
                anoTurmaObjetivosAprendizagem = (anoTurma + ANO_TURMA_EJA_INICIA_A_PARTIR_DE);
            else anoTurmaObjetivosAprendizagem = anoTurma;

            return ((AnoTurma)anoTurmaObjetivosAprendizagem).Name();
        }

        private string[] ObterAnosFiltroObjetivosAprendizagem(Turma turma, bool ehEnsinoEspecial, string[] retornoDefault)
        {
            if (ehEnsinoEspecial)
              return ANOS_OBJETIVO_APRENDIZAGEM_ENSINO_ESPECIAL;
            if (turma.EhEJA())
                return ANOS_OBJETIVO_APRENDIZAGEM_EJA;
            if (turma.EhTurmaPrograma())
                return Array.Empty<string>();
            return retornoDefault;
        }

        private async Task<long[]> ObterIdsJurema(long componenteCurricularId, bool ehEnsinoEspecial)
        {
            const long LINGUA_PORTUGUESA_JUREMA_ENSINO_ESPECIAL = 11;
            const long LINGUA_PORTUGUESA_JUREMA = 6;
            const long ID_COMPONENTE_LINGUA_PORTUGUESA = 138;
            var ehLinguaPortuguesa = componenteCurricularId == ID_COMPONENTE_LINGUA_PORTUGUESA;

            if (ehLinguaPortuguesa)
                if (ehEnsinoEspecial)
                    return new long[] { LINGUA_PORTUGUESA_JUREMA_ENSINO_ESPECIAL };
                else
                    return new long[] { LINGUA_PORTUGUESA_JUREMA };

            return await mediator.Send(new ObterJuremaIdsPorComponentesCurricularIdQuery(componenteCurricularId));
        }
    }
}
