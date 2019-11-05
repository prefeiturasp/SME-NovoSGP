import React from 'react';

// Styles
import { SmallText, VerticalCentered } from '../styles';

// Componentes
import BootstrapRow from './BootstrapRow';

export default function WarningText() {
  return (
    <BootstrapRow>
      <VerticalCentered className="col-lg-12">
        <SmallText>
          Se não selecionar uma data fim, será considerado o fim do ano letivo.
        </SmallText>
      </VerticalCentered>
    </BootstrapRow>
  );
}
