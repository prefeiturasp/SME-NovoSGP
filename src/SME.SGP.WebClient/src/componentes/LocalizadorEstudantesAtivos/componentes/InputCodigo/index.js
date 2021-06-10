import { Button, Input } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import Loader from '~/componentes/loader';
import { InputRFEstilo } from './styles';

const InputCodigo = props => {
  const {
    pessoaSelecionada,
    onSelect,
    onChange,
    desabilitado,
    exibirLoader,
  } = props;

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

  const onChangeCodigo = value => {
    if (!exibirLoader) {
      setValor(value);
      onChange(value);
    }
  };

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.alunoCodigo);
  }, [pessoaSelecionada]);

  const someteNumero = v => {
    return String(v).replace(/\D/g, '');
  };

  return (
    <Loader loading={exibirLoader}>
      <InputRFEstilo>
        <Input
          value={valor}
          placeholder="Digite o Código EOL"
          onChange={e => {
            const valorSomenteNumero = someteNumero(e.target.value);
            onChangeCodigo(valorSomenteNumero);
          }}
          onPressEnter={e => {
            if (!exibirLoader && e.target.value) {
              const valorSomenteNumero = someteNumero(e.target.value);
              onSubmitCodigo(valorSomenteNumero);
            }
          }}
          suffix={botao}
          disabled={desabilitado}
          allowClear
        />
      </InputRFEstilo>
    </Loader>
  );
};

InputCodigo.propTypes = {
  pessoaSelecionada: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onSelect: PropTypes.func,
  onChange: PropTypes.func,
  desabilitado: PropTypes.bool,
  exibirLoader: PropTypes.bool,
};

InputCodigo.defaultProps = {
  pessoaSelecionada: {},
  onSelect: () => {},
  onChange: () => {},
  desabilitado: false,
  exibirLoader: false,
};

export default InputCodigo;
