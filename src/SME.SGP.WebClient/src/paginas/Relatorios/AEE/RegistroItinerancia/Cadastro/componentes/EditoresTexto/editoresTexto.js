import React, { useEffect, useState } from 'react';

import { JoditEditor } from '~/componentes';
import ServicoRegistroItineranciaAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoRegistroItineranciaAEE';

const EditoresTexto = ({dados}) => {
  const [acompanhamentoSituacao, setAcompanhamentoSituacao] = useState();
  const [encaminhamentos, setEncaminhamentos] = useState();

  return (
    <>
      {dados &&
        dados.map(questao => {
          return (
            <div className="row mb-4">
              <div className="col-12">
                <JoditEditor
                  label={questao.descricao}
                  value=""
                  name={questao.descricao + questao.questaoId}
                  id={questao.questaoId}
                  onChange={e => setAcompanhamentoSituacao(e)}
                />
              </div>
            </div>
          );
        })}
    </>
  );
};

export default EditoresTexto;
