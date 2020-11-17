import React, { useEffect } from 'react';

const Sondagem = ({ history }) => {
  useEffect(() => {
    window.open(`http://localhost:61540/sgp?redirect=/Relatorios/Sondagem`);
    history.push('/');
  }, [history]);

  return <div>sondagem</div>;
};

export default Sondagem;
