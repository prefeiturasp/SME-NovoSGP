import React, { useState } from 'react';
import { MaisMenos } from './fechamento-bimestre-lista.css';

const BotaoExpandir = () => {
  const [expandido, setExpandido] = useState(false);

  return (
    <a onClick={() => setExpandido(!expandido)}>
      {expandido ?
        <MaisMenos className="fas fa-minus-circle" />
        :
        <MaisMenos className="fas fa-plus-circle" />
      }
    </a>
  );
}

export default BotaoExpandir;
