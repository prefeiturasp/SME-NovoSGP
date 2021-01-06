import { Button, Input } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { InputRFEstilo } from './styles';

const InputTurma = props => {
  const { pessoaSelecionada, onSelect, onChange, desabilitado } = props;

  const [valor, setValor] = useState('');

  const onSubmitCodigo = codigo => {
    onSelect({ codigo });
  };

  const botao = (
    <Button
      onClick={() => onSubmitCodigo(valor)}
      disabled={!valor || desabilitado}
      type="link"
    >
      <i className="fa fa-search fa-lg" />
    </Button>
  );

  const onChangeCodigo = e => {
    setValor(e.target.value);
    onChange(e.target.value);
  };

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.turmaCodigo);
  }, [pessoaSelecionada]);

  return (
    <>
      <InputRFEstilo>
        <Input
          value={valor}
          placeholder="Digite o Código da Turma"
          onChange={onChangeCodigo}
          onPressEnter={onSubmitCodigo}
          suffix={botao}
          disabled={desabilitado}
          allowClear
        />
      </InputRFEstilo>
    </>
  );
};

InputTurma.propTypes = {
  pessoaSelecionada: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onSelect: PropTypes.func,
  onChange: PropTypes.func,
  desabilitado: PropTypes.bool,
};

InputTurma.defaultProps = {
  pessoaSelecionada: {},
  onSelect: () => {},
  onChange: () => {},
  desabilitado: false,
};

export default InputTurma;
