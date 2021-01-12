import { Steps } from 'antd';
import React from 'react';
import { ContainerStepsEncaminhamento } from '../../../encaminhamentoAEECadastro.css';
import DescricaoEncaminhamentoCollapse from './DescricaoEncaminhamentoCollapse/descricaoEncaminhamentoCollapse';
import InformacoesEscolaresCollapse from './InformacoesEscolaresCollapse/informacoesEscolaresCollapse';

const { Step } = Steps;

const DadosSecaoEncaminhamento = () => {
  return (
    <ContainerStepsEncaminhamento direction="vertical" current={1}>
      <Step status="finish" title={<InformacoesEscolaresCollapse />} />
      <Step status="process" title={<DescricaoEncaminhamentoCollapse />} />
    </ContainerStepsEncaminhamento>
  );
};

export default DadosSecaoEncaminhamento;
