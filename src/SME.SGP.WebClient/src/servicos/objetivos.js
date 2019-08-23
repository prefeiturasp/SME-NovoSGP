const API_JUREMA =
  'https://curriculo.sme.prefeitura.sp.gov.br/api/v1/learning_objectives';

export async function listarObjetivosAprendizagem() {
  return fetch(`${API_JUREMA}`).then(objetivos => objetivos.json());
}
