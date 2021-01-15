import api from '~/servicos/api';

const urlPadrao = `v1/componentes-curriculares`;

class ServicoComponentesCurriculares {
  obterComponetensCuriculares = (
    codigoUe,
    modalidade,
    anoLetivo,
    anosEscolares,
    turmasPrograma
  ) => {
    const url = `${urlPadrao}/ues/${codigoUe}/modalidades/${modalidade}/anos/${anoLetivo}/anos-escolares?anosEscolares=${anosEscolares.join(
      '&anosEscolares=',
      anosEscolares
    )}&turmasPrograma=${!!turmasPrograma}`;
    return api.get(url);
  };

  obterComponetensCuricularesRegencia = turmaId => {
    return api.get(`${urlPadrao}/turmas/${turmaId}/regencia/componentes`);
  };

  obterComponentesPorListaDeTurmas = turmasId => {
    const url = `v1/professores/disciplinas/turmas`;
    return api.post(url, turmasId);
  };
}

export default new ServicoComponentesCurriculares();
