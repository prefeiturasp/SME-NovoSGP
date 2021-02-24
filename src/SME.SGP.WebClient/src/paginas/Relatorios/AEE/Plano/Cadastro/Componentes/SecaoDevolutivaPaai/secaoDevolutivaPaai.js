import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Editor } from '~/componentes';
import {
  setDevolutivaEmEdicao,
  setParecerPAAI,
} from '~/redux/modulos/planoAEE/actions';

const SecaoDevolutivaPaai = () => {
  const parecerPAAI = useSelector(store => store.planoAEE.parecerPAAI);
  const dadosDevolutiva = useSelector(store => store.planoAEE.dadosDevolutiva);

  const dispatch = useDispatch();

  const mudarDescricaoPAAI = texto => {
    dispatch(setDevolutivaEmEdicao(true));
    dispatch(setParecerPAAI(texto));
  };

  return (
    <div className="mb-3">
      <Editor
        label="Devolutiva do PAAI"
        onChange={mudarDescricaoPAAI}
        inicial={dadosDevolutiva?.parecerPAAI || parecerPAAI || ''}
        desabilitar={dadosDevolutiva?.podeEditarParecerPAAI}
        removerToolbar={dadosDevolutiva?.podeEditarParecerPAAI}
      />
    </div>
  );
};

export default SecaoDevolutivaPaai;
