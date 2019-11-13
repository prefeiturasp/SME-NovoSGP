import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { Input, Button } from 'antd';

// Styles
import { InputRFEstilo } from './styles';

function InputRF({ pessoaSelecionada, onSelect }) {
  const [valor, setValor] = useState('');

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.rf);
  }, [pessoaSelecionada]);

  const onSubmitRF = rf => {
    onSelect({ rf });
  };

  const botao = (
    <Button onClick={() => onSubmitRF(valor)} disabled={!valor} type="link">
      <i className="fa fa-search fa-lg" />
    </Button>
  );

  return (
    <InputRFEstilo>
      <Input
        value={valor}
        placeholder="Digite o RF"
        onChange={e => setValor(e.target.value)}
        onPressEnter={e => onSubmitRF(e.target.value)}
        suffix={botao}
      />
    </InputRFEstilo>
  );
}

InputRF.propTypes = {
  pessoaSelecionada: PropTypes.objectOf(PropTypes.object),
  onSelect: PropTypes.func,
};

InputRF.defaultProps = {
  pessoaSelecionada: {},
  onSelect: () => null,
};

export default InputRF;
