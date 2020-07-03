import api from '~/servicos/api';

class ServicoCadastroAula {
  obterPorId = idAula => {
    const url = `v1/calendarios/professores/aulas/${idAula}`;
    return api.get(url);
  };

  salvar = (id, aula, ehRegencia) => {
    const parms = {
      ...aula,
      CodigoComponenteCurricular: aula.disciplinaId,
      NomeComponenteCurricular: aula.disciplinaNome,
      CodigoTurma: aula.turmaId,
      codigoUe: aula.ueId,
      ehRegencia,
    };

    let metodo = 'post';
    let url = 'v1/calendarios/professores/aulas';
    if (id > 0) {
      metodo = 'put';
      url = `${url}/${id}`;
    }
    return api[metodo](url, parms);
  };

  obterGradePorComponenteETurma = (
    turmaId,
    componenteId,
    dataAula,
    aulaId,
    ehRegencia
  ) => {
    const url = `v1/calendarios/professores/aulas/${aulaId}/turmas/${turmaId}/componente-curricular/${componenteId}?dataAula=${dataAula.format(
      'YYYY-MM-DD'
    )}&ehRegencia=${ehRegencia}`;
    return api.get(url);
  };

  obterRecorrenciaPorIdAula = id => {
    return api.get(`v1/calendarios/professores/aulas/${id}/recorrencias/serie`);
  };

  excluirAula = (id, tipoRecorrencia, nomeComponente) => {
    return api.delete(
      `v1/calendarios/professores/aulas/${id}/recorrencias/${tipoRecorrencia}/disciplinaNome/${btoa(
        nomeComponente
      )}`
    );
  };
}

export default new ServicoCadastroAula();
