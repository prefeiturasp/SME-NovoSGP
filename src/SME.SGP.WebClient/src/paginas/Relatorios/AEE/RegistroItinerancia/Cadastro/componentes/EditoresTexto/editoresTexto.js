import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { JoditEditor } from '~/componentes';
import { setQuestoesItinerancia } from '~/redux/modulos/itinerancia/action';

const EditoresTexto = props => {
  const {form} = props;
  const dispatch = useDispatch();
  const dados = useSelector(store => store.itinerancia.questoesItinerancia);

  const setAcompanhamentoSituacao = (valor, questao) => {
    questao.resposta = valor;
    dispatch(setQuestoesItinerancia([...dados]));
  };

  return (
    <>
      {dados &&
        dados.map(questao => {
          return (
            <div className="row mb-4" key={questao.questaoId}>
              <div className="col-12">
                <JoditEditor
                  form={form}
                  label={questao.descricao}
                  value={questao.resposta}
                  name={`questao-${questao.questaoId}`}
                  id={questao.questaoId}
                  onChange={e => setAcompanhamentoSituacao(e, questao)}
                />
              </div>
            </div>
          );
        })}
    </>
  );
};

export default EditoresTexto;
