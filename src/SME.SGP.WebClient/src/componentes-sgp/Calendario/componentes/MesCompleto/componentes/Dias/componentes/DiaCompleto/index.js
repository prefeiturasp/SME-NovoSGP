import React, { useMemo, useEffect } from 'react';
import t from 'prop-types';

// Estilos
import { DiaCompletoWrapper } from './styles';

// Componentes
import { Loader } from '~/componentes';

// Componentes internos
import AlertaDentroPeriodo from './componentes/AlertaPeriodoEncerrado';

function DiaCompleto({ dia, eventos, carregandoDia, diasPermitidos }) {
  const deveExibir = useMemo(
    () => dia && !!diasPermitidos.find(x => x.toString() === dia.toString()),
    [dia, diasPermitidos]
  );

  const dadosDia = useMemo(() => {
    return eventos.length > 0 && dia instanceof Date
      ? eventos.filter(diaAtual => diaAtual.dia === dia.getDate())[0]
      : null;
  }, [dia, eventos]);

  useEffect(() => {
    console.log('Dados dia aberto', dadosDia);
  }, [dadosDia, dia, eventos]);

  return (
    <DiaCompletoWrapper className={`${deveExibir && `visivel`}`}>
      {deveExibir && (
        <Loader loading={carregandoDia} tip="Carregando...">
          <AlertaDentroPeriodo
            exibir={dadosDia.dados && !dadosDia.dados.mensagemPeriodoEncerrado}
          />
        </Loader>
      )}
    </DiaCompletoWrapper>
  );
}

DiaCompleto.propTypes = {
  dia: t.oneOfType([t.any]),
  eventos: t.oneOfType([t.any]),
  diasPermitidos: t.oneOfType([t.any]),
  carregandoDia: t.bool,
};

DiaCompleto.defaultProps = {
  dia: {},
  eventos: {},
  diasPermitidos: [],
  carregandoDia: false,
};

export default DiaCompleto;
