import React from 'react';

// Ant
import { Icon } from 'antd';

// Estilos
import {
  CollapseEstilizado,
  PainelEstilizado,
  IconeEstilizado,
} from './styles';

function PainelCollapse({ children, ...props }) {
  const renderizarIcone = painelProps =>
    painelProps.isActive ? <Icon type="up" /> : <Icon type="down" />;

  return (
    <CollapseEstilizado
      expandIconPosition="right"
      expandIcon={painelProps => renderizarIcone(painelProps)}
      {...props}
    >
      {children}
    </CollapseEstilizado>
  );
}

function Painel({ children, ...props }) {
  return <PainelEstilizado {...props}>{children}</PainelEstilizado>;
}

PainelCollapse.Painel = Painel;

export default PainelCollapse;
