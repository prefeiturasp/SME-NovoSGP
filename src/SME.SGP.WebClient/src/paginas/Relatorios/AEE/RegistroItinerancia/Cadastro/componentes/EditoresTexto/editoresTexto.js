import React, { useState } from 'react';

import { JoditEditor } from '~/componentes';

const EditoresTexto = () => {
  const [acompanhamentoSituacao, setAcompanhamentoSituacao] = useState();
  const [encaminhamentos, setEncaminhamentos] = useState();

  return (
    <>
      <div className="row mb-4">
        <div className="col-12">
          <JoditEditor
            label="Acompanhamento da situação"
            value={acompanhamentoSituacao}
            name="acompanhamentoSituacao"
            id="acompanhamentoSituacao"
            onChange={e => setAcompanhamentoSituacao(e)}
          />
        </div>
      </div>
      <div className="row mb-4">
        <div className="col-12">
          <JoditEditor
            label="Encaminhamentos"
            value={encaminhamentos}
            name="encaminhamentos"
            id="encaminhamentos"
            onChange={e => setEncaminhamentos(e)}
          />
        </div>
      </div>
    </>
  );
};

export default EditoresTexto;
