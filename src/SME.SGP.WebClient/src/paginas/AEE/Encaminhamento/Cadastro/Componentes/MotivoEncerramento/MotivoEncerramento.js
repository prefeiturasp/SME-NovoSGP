import { Input } from 'antd';
import React from 'react';
import { useSelector } from 'react-redux';
import { Label } from '~/componentes';
import situacaoAEE from '~/dtos/situacaoAEE';

const { TextArea } = Input;

const MotivoEncerramento = () => {
  const dadosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.dadosEncaminhamento
  );

  return dadosEncaminhamento?.situacao === situacaoAEE.Encerrado &&
    dadosEncaminhamento?.motivoEncerramento ? (
    <div className="mt-3 mb-3">
      <Label text="Motivo do encerramento do encaminhamento" />
      <TextArea
        id="motivo-encerramento"
        autoSize={{ minRows: 4 }}
        value={dadosEncaminhamento?.motivoEncerramento}
        disabled
      />
    </div>
  ) : (
    ''
  );
};

export default MotivoEncerramento;
