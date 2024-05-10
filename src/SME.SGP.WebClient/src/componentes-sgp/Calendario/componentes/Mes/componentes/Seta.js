import React from 'react';
import t from 'prop-types';

function Seta({ estaAberto }) {
  return (
    <i
      className={`fas ${
        estaAberto ? 'fa-chevron-down' : 'fa-chevron-right text-white'
      } `}
    />
  );
}

Seta.propTypes = {
  estaAberto: t.bool,
};

Seta.defaultProps = {
  estaAberto: false,
};

export default Seta;
