import FiltroPlanoAnualDto from '~/dtos/filtroPlanoAnualDto';
import Service from '~/servicos/Paginas/PlanoAnualServices';
import _ from 'lodash';
import { erro } from '~/servicos/alertas';

export default class PlanoAnualHelper {
  static async verificarEdicao(filtroPlanoAnualDto, ehEja) {
    const validarExistente = await Service.validarPlanoExistente(
      filtroPlanoAnualDto
    )
      .then(res => !res)
      .catch(() => {
        erro(
          `Não foi possivel obter os dados do ${
            ehEja ? 'plano semestral' : 'plano anual'
          }`
        );
        return false;
      });

    return validarExistente;
  }

  static async ObterDisciplinasPlano(codigoRf, turmaId) {
    const disciplinas = await Service.getDisciplinasProfessor(codigoRf, turmaId)
      .then(res => res)
      .catch(() => {
        erro(`Não foi possivel obter as disciplinas do professor`);
        return null;
      });

    return disciplinas;
  }

  static async ObterDiscplinasObjetivos(turmaId, disciplinaSelecionada) {
    const disciplinas = await Service.getDisciplinasProfessorObjetivos(turmaId)
      .then(res => res)
      .catch(() => {
        erro(`Não foi possivel obter as disciplinas do professor`);
        return null;
      });

    if (disciplinaSelecionada.regencia) return disciplinas;

    const retorno =
      disciplinas &&
      disciplinas.find(x => x.codigo === disciplinaSelecionada.codigo);

    return [retorno];
  }

  static ObtenhaBimestres = (
    disciplinas = [],
    ehEdicao,
    filtroPlanoAnualDto,
    anoEscolar,
    layoutEspecial,
    ehEja
  ) => {
    let bimestres = [];

    const qtdBimestres = ehEja ? 2 : 4;

    for (let i = 1; i <= qtdBimestres; i++) {
      const Nome = `${i}º Bimestre`;

      const objetivo = '';

      const bimestre = {
        anoLetivo: filtroPlanoAnualDto.anoLetivo,
        anoEscolar: anoEscolar,
        escolaId: filtroPlanoAnualDto.escolaId,
        turmaId: filtroPlanoAnualDto.turmaId,
        indice: i,
        nome: Nome,
        materias: disciplinas,
        objetivo: objetivo,
        paraEnviar: false,
        recarregarPlanoAnual: false,
        ehEdicao,
        LayoutEspecial: layoutEspecial,
        ehExpandido: ehEdicao,
        jaSincronizou: false,
      };

      bimestres[i] = bimestre;
    }

    return bimestres;
  };
}
