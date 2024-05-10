import React, { useMemo } from 'react';
import t from 'prop-types';

// Estilo
import { MesCompletoWrapper } from './styles';

// Componentes
import { Loader } from '~/componentes';

// Componentes internos
import DiasDaSemana from './componentes/DiasDaSemana';
import Dias from './componentes/Dias';

function MesCompleto({
  mes,
  eventos,
  mesesPermitidos,
  diaSelecionado,
  onClickDia,
  carregandoMes,
  carregandoDia,
  permissaoTela,
  tipoCalendarioId,
}) {
  const deveExibir = useMemo(
    () => mes && mesesPermitidos.indexOf(mes.numeroMes) >= 0,
    [mes, mesesPermitidos]
  );

  return (
    <MesCompletoWrapper className={`${mes.nome} ${deveExibir && `visivel`}`}>
      {deveExibir && (
        <Loader loading={carregandoMes} tip="Carregando...">
          <DiasDaSemana />
          <Dias
            eventos={
              eventos?.meses?.filter(evt => evt.numeroMes === mes.numeroMes)[0]
            }
            mesSelecionado={mes}
            onClickDia={onClickDia}
            carregandoDia={carregandoDia}
            diaSelecionado={diaSelecionado}
            permissaoTela={permissaoTela}
            tipoCalendarioId={tipoCalendarioId}
          />
        </Loader>
      )}
    </MesCompletoWrapper>
  );
}

MesCompleto.propTypes = {
  mes: t.oneOfType([t.any]),
  eventos: t.oneOfType([t.any]),
  mesesPermitidos: t.oneOfType([t.any]),
  diaSelecionado: t.oneOfType([t.any]),
  onClickDia: t.func,
  carregandoMes: t.bool,
  carregandoDia: t.bool,
  permissaoTela: t.oneOfType([t.any]),
  tipoCalendarioId: t.oneOfType([t.any]),
};

MesCompleto.defaultProps = {
  mes: {},
  eventos: {},
  mesesPermitidos: [],
  diaSelecionado: {},
  onClickDia: () => {},
  carregandoMes: false,
  carregandoDia: false,
  permissaoTela: {},
  tipoCalendarioId: null,
};

export default MesCompleto;
