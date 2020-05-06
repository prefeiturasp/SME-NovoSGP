import React from 'react';

// Componentes
import { Base } from '~/componentes';

function SemEventos() {
  return (
    <div
      className="d-flex w-100 h-100 justify-content-center d-flex align-items-center fade show pb-2 pt-4"
      style={{ fontSize: 25, color: Base.CinzaBotao }}
    >
      Sem eventos neste dia
    </div>
  );
}

export default SemEventos;
