import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';

import { Editor } from '~/componentes';
import {
  setDevolutivaEmEdicao,
  setParecerCoordenacao,
} from '~/redux/modulos/planoAEE/actions';

const SecaoDevolutivaCoordenacao = ({ desabilitarDevolutivaCordenacao }) => {
  const parecerCoordenacao = useSelector(
    store => store.planoAEE.parecerCoordenacao
  );
  const dadosDevolutiva = useSelector(store => store.planoAEE.dadosDevolutiva);

  const dispatch = useDispatch();

  const mudarDescricaoCordenacao = texto => {
    dispatch(setDevolutivaEmEdicao(true));
    dispatch(setParecerCoordenacao(texto));
  };

  return (
    <div className="mb-3">
      <Editor
        label="Devolutiva da coordenação"
        onChange={mudarDescricaoCordenacao}
        inicial={
          dadosDevolutiva?.parecerCoordenacao || parecerCoordenacao || ''
        }
        desabilitar={desabilitarDevolutivaCordenacao}
        removerToolbar={desabilitarDevolutivaCordenacao}
      />
    </div>
  );
};

SecaoDevolutivaCoordenacao.defaultProps = {
  desabilitarDevolutivaCordenacao: false,
};

SecaoDevolutivaCoordenacao.propTypes = {
  desabilitarDevolutivaCordenacao: PropTypes.bool,
};

export default SecaoDevolutivaCoordenacao;
