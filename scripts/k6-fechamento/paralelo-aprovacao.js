import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';
import { textSummary } from 'https://jslib.k6.io/k6-summary/0.1.0/index.js';
import { htmlReport } from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';
import { ENDPOINTS, PEAK_VUS, rampingStages, REPORT_SUFFIX, headers } from './config.js';

// Variante do "paralelo.js" restrita aos 2 endpoints do card 155456 (nota de fechamento
// em aprovação): fechamentos/turmas e fechamentos/turmas/listar. Não chama fechamentos/finais
// (card 155454, do Guilherme) para não gerar carga à toa num endpoint que essa branch não mudou.

export const options = {
  scenarios: {
    paralelo: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: rampingStages(PEAK_VUS),
      gracefulRampDown: '10s',
      gracefulStop: '0s',
    },
  },
  thresholds: {
    http_req_failed: ['rate<0.01'],
  },
};

const trendBuscarTurma = new Trend('duration_fechamentos_turmas', true);
const trendListarTurma = new Trend('duration_fechamentos_turmas_listar', true);

export default function () {
  const opts = headers();

  const responses = http.batch([
    ['GET', ENDPOINTS.buscarTurma.url(), null, { ...opts, tags: { endpoint: ENDPOINTS.buscarTurma.name } }],
    ['GET', ENDPOINTS.listarTurma.url(), null, { ...opts, tags: { endpoint: ENDPOINTS.listarTurma.name } }],
  ]);

  const [resBuscarTurma, resListarTurma] = responses;

  trendBuscarTurma.add(resBuscarTurma.timings.duration);
  check(resBuscarTurma, {
    'fechamentos/turmas -> status 200': (r) => r.status === 200,
  });

  trendListarTurma.add(resListarTurma.timings.duration);
  check(resListarTurma, {
    'fechamentos/turmas/listar -> status 200': (r) => r.status === 200,
  });

  sleep(0.2);
}

export function handleSummary(data) {
  return {
    stdout: textSummary(data, { indent: ' ', enableColors: true }),
    [`relatorio-paralelo-aprovacao${REPORT_SUFFIX}.html`]: htmlReport(data, {
      title: 'Fechamento - Nota em Aprovacao (155456)',
    }),
  };
}
