import React, { useEffect } from 'react';
import PropTypes from 'prop-types';
import { obterUrlSondagem } from '~/servicos/variaveis';

const Sondagem = ({ history }) => {
  useEffect(() => {
    (async () => {
      const url = await obterUrlSondagem();
      window.open(`${url}/sgp?redirect=/Relatorios/Sondagem`);
      history.push('/');
    })();
  }, [history]);

  return <div>sondagem</div>;
};

Sondagem.propTypes = {
  history: PropTypes.func.isRequired,
};

export default Sondagem;
