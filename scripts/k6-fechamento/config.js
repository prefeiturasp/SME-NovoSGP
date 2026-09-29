// Configuracao compartilhada pelos testes de performance dos endpoints de fechamento.
// Todos os parametros podem ser sobrescritos via -e NOME=valor na linha de comando do k6.

export const BASE_URL = __ENV.BASE_URL || 'https://hom-novosgp.sme.prefeitura.sp.gov.br';
export const TOKEN = __ENV.TOKEN || '';

export const TURMA_CODIGO = __ENV.TURMA_CODIGO || '3018920';
export const DISCIPLINA_CODIGO = __ENV.DISCIPLINA_CODIGO || '1213';
export const BIMESTRE = __ENV.BIMESTRE || '3';
export const SEMESTRE = __ENV.SEMESTRE || '0';
export const EH_REGENCIA = __ENV.EH_REGENCIA || 'true';

// Sufixo opcional para diferenciar os relatorios entre execucoes (ex.: "antes"/"depois"),
// evitando que uma rodada sobrescreva o relatorio HTML da outra.
export const REPORT_SUFFIX = __ENV.REPORT_SUFFIX ? `-${__ENV.REPORT_SUFFIX}` : '';

// Pico maximo de usuarios virtuais simultaneos durante o teste de carga (ramping).
// 30 VUs saturou a versao "antes" da otimizacao (timeouts em cascata, respostas de
// ate 60s) — 15 mantem uma curva de carga legivel sem estourar o ambiente de homologacao.
export const PEAK_VUS = Number(__ENV.PEAK_VUS || 15);

// Estagios de ramping somando exatamente 5 minutos (limite para nao sobrecarregar o
// ambiente de homologacao): sobe gradualmente ate a metade do pico, sobe ate o pico,
// segura no pico, e desce gradualmente ate zero.
export function rampingStages(peakVus) {
  const half = Math.max(1, Math.round(peakVus / 2));
  return [
    { duration: '30s', target: half }, // 0:00 -> 0:30
    { duration: '1m', target: peakVus }, // 0:30 -> 1:30
    { duration: '2m', target: peakVus }, // 1:30 -> 3:30 (sustentado no pico)
    { duration: '1m', target: half }, // 3:30 -> 4:30
    { duration: '30s', target: 0 }, // 4:30 -> 5:00
  ];
}

export function headers() {
  if (!TOKEN) {
    throw new Error(
      'Parametro TOKEN nao informado. Execute o script passando -e TOKEN="<bearer token>"'
    );
  }
  return {
    headers: {
      Authorization: `Bearer ${TOKEN}`,
      Accept: 'application/json',
    },
  };
}

export const ENDPOINTS = {
  buscarTurma: {
    name: 'fechamentos_turmas',
    url: () =>
      `${BASE_URL}/api/v1/fechamentos/turmas?turmaCodigo=${TURMA_CODIGO}&disciplinaCodigo=${DISCIPLINA_CODIGO}&bimestre=${BIMESTRE}&semestre=${SEMESTRE}`,
  },
  listarTurma: {
    name: 'fechamentos_turmas_listar',
    url: () =>
      `${BASE_URL}/api/v1/fechamentos/turmas/listar?turmaCodigo=${TURMA_CODIGO}&componenteCurricularCodigo=${DISCIPLINA_CODIGO}&bimestre=${BIMESTRE}&semestre=${SEMESTRE}`,
  },
  fechamentosFinais: {
    name: 'fechamentos_finais',
    url: () =>
      `${BASE_URL}/api/v1/fechamentos/finais?DisciplinaCodigo=${DISCIPLINA_CODIGO}&TurmaCodigo=${TURMA_CODIGO}&ehRegencia=${EH_REGENCIA}&semestre=${SEMESTRE}`,
  },
};
