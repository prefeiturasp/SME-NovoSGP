import axios from 'axios';

const urlBase = () =>
  axios
    .get('/../../../configuracoes/variaveis.json')
    .then(response => {
      return response.data.API_URL;
    })
    .catch(() => {
      window.location.href = '/erro';
    });

const obterTrackingID = () =>
  axios
    .get('/../../../configuracoes/variaveis.json')
    .then(response => {
      return response.data.TRACKING_ID;
    })
    .catch(() => {
      window.location.href = '/erro';
    });

const obterUrlSondagem = () =>
  axios
    .get('/../../../configuracoes/variaveis.json')
    .then(response => {
      return response.data.URL_SONDAGEM;
    })
    .catch(() => {
      window.location.href = '/erro';
    });

export { urlBase, obterTrackingID, obterUrlSondagem };
