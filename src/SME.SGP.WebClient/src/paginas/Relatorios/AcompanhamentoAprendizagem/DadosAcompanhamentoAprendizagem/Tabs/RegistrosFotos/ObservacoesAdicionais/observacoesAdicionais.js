import React, { useState } from 'react';
import { JoditEditor } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';

const ObservacoesAdicionais = () => {
  const [exibir, setExibir] = useState(true);

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="observacoes-adicionais-collapse"
        onClick={onClickExpandir}
        titulo="Observações adicionais"
        indice="observacoes-adicionais"
        show={exibir}
        alt="observacoes-adicionais"
      >
        {exibir ? (
          <JoditEditor
            id="observacoes-adicionais-editor"
            // value={dados}
            // onChange={onChange}
          />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

export default ObservacoesAdicionais;
