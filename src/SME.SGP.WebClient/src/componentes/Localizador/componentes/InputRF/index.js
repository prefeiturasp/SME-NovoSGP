import React, { useState, useEffect } from 'react';

// Componentes
import { Input, Button } from 'antd';

// Styles
import { InputRFEstilo } from './styles';

export default function InputRF({ pessoaSelecionada }) {
  const [valor, setValor] = useState('');

  useEffect(() => {
    setValor(pessoaSelecionada.rf);
  }, [pessoaSelecionada]);

  const botao = (
    <Button onClick={() => alert('nd')} disabled={!valor} type="link">
      <i className="fa fa-search fa-lg" />
    </Button>
  );

  return (
    <InputRFEstilo>
      <Input
        value={valor}
        placeholder="Digite o RF"
        onChange={e => setValor(e.target.value)}
        suffix={botao}
      />
    </InputRFEstilo>
  );
}
