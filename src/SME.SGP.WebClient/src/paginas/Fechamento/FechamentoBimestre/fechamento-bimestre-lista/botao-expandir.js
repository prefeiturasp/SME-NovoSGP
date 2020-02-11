import React, { useState } from 'react';
import { MaisMenos } from './fechamento-bimestre-lista.css';

const BotaoExpandir = props => {
  const { index, idLinhaRegencia } = props;
  const [expandido, setExpandido] = useState(false);

  const clickExpandirRetrair = () => {
    setExpandido(!expandido);
    const linhaRegencia = window.document.getElementById(idLinhaRegencia);
    if (linhaRegencia) {
      const display = linhaRegencia.style.display;
      linhaRegencia.style.display = display === 'none' ? 'contents' : 'none';
    }
  };

  return (
    <a onClick={() => clickExpandirRetrair()}>
      {expandido ? (
        <MaisMenos className="fas fa-minus-circle" />
      ) : (
        <MaisMenos className="fas fa-plus-circle" />
      )}
    </a>
  );
};

export default BotaoExpandir;
