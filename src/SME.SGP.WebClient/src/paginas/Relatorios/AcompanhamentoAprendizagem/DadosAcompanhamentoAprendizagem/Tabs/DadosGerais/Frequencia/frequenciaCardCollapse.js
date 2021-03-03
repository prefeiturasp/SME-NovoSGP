import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';

const FrequenciaCardCollapse = () => {
  const [exibir, setExibir] = useState(true);

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="frequencia-acompanhamento-aprendizagem-collapse"
        onClick={onClickExpandir}
        titulo="Frequência"
        indice="frequencia-acompanhamento-aprendizagem"
        show={exibir}
        alt="frequencia-acompanhamento-aprendizagem"
      >
        Frequencia
      </CardCollapse>
    </div>
  );
};

export default FrequenciaCardCollapse;
