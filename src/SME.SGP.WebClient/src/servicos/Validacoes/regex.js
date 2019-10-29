/* eslint-disable func-names */
import * as Yup from 'yup';

Yup.addMethod(
  Yup.mixed,
  'contem',
  // eslint-disable-next-line func-names
  function(padrao, mensagem) {
    // eslint-disable-next-line func-names
    return this.test('contem', mensagem, function(valor) {
      return valor && valor.match(padrao);
    });
  }
);

Yup.addMethod(
  Yup.mixed,
  'naoContem',
  // eslint-disable-next-line func-names
  function(padrao, mensagem) {
    // eslint-disable-next-line func-names
    return this.test('naoContem', mensagem, function(valor) {
      return valor && !valor.match(padrao);
    });
  }
);
