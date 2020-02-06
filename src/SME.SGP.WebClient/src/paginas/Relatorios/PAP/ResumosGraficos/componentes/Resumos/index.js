import React, { lazy, Suspense } from 'react';

// Componentes
import { PainelCollapse } from '~/componentes';

function Resumos() {
  const TabelaFrequencia = lazy(() => import('./componentes/TabelaFrequencia'));
  const TabelaTotalEstudantes = lazy(() =>
    import('./componentes/TabelaTotalEstudantes')
  );

  return (
    <>
      <PainelCollapse>
        <PainelCollapse.Painel temBorda header="Total de estudantes">
          <Suspense fallback={<h1>Carregando...</h1>}>
            <TabelaTotalEstudantes />
          </Suspense>
        </PainelCollapse.Painel>
      </PainelCollapse>
      <PainelCollapse>
        <PainelCollapse.Painel temBorda header="Frequência">
          <Suspense fallback={<h1>Carregando...</h1>}>
            <TabelaFrequencia />
          </Suspense>
        </PainelCollapse.Painel>
      </PainelCollapse>
    </>
  );
}

export default Resumos;
