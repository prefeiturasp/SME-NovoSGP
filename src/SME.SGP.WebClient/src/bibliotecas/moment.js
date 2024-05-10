import moment from 'moment-with-locales-es6';

moment.locale('pt-br');
window.moment = moment;

Date.prototype.toISOString = function () {
  return moment(this).format('YYYY-MM-DDTHH:mm:ss');
};
