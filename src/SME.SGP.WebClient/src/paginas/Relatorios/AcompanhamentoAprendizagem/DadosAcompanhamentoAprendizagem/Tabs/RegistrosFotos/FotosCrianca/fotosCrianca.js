import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';

const FotosCrianca = () => {
  const [exibir, setExibir] = useState(false);

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="fotos-crianca-collapse"
        onClick={onClickExpandir}
        titulo="Fotos da criança"
        indice="fotos-crianca"
        show={exibir}
        alt="fotos-crianca"
      >
        Fotos da criança
      </CardCollapse>
    </div>
  );
};

export default FotosCrianca;
