import React, { useMemo, useCallback } from 'react';
import t from 'prop-types';
import shortid from 'shortid';
import _ from 'lodash';

// Redux
import { useSelector } from 'react-redux';

// Componentes internos
import Dia from './componentes/Dia';
import DiaCompleto from './componentes/DiaCompleto';

// Estilos
import { DiasWrapper } from './styles';

function Dias({
  mesSelecionado,
  eventos,
  diaSelecionado,
  onClickDia,
  carregandoDia,
  permissaoTela,
  tipoCalendarioId,
}) {
  const usuario = useSelector(estado => estado.usuario);

  const diasParaRenderizar = useMemo(() => {
    if (!mesSelecionado) return [];

    const dias = [];
    const data = new Date(
      usuario.turmaSelecionada.anoLetivo,
      mesSelecionado.numeroMes - 1,
      1
    );

    data.setDate(data.getDate() - data.getDay() - 1);

    for (let i = 0; i < 42; i += 1) {
      dias.push(new Date(data.setDate(data.getDate() + 1)));
    }

    return dias;
  }, [mesSelecionado, usuario.turmaSelecionada]);

  const diasPermitidos = useMemo(() => {
    const semanas = [];
    const diasParaRenderizarClone = _.clone(diasParaRenderizar);
    for (let i = 1; i < diasParaRenderizarClone.length; i += 1) {
      semanas.push(diasParaRenderizarClone.splice(0, 7));
    }

    return semanas;
  }, [diasParaRenderizar]);

  const obterSemanaPeloIndice = useCallback(
    indice => {
      if (indice > 7 && indice < 14) {
        return diasPermitidos[1];
      }

      if (indice > 14 && indice < 21) {
        return diasPermitidos[2];
      }

      if (indice > 21 && indice < 28) {
        return diasPermitidos[3];
      }

      if (indice > 28 && indice < 35) {
        return diasPermitidos[4];
      }

      if (indice > 35 && indice < 42) {
        return diasPermitidos[5];
      }

      return diasPermitidos[0];
    },
    [diasPermitidos]
  );

  return (
    <DiasWrapper>
      {diasParaRenderizar &&
        diasParaRenderizar.map((dia, indice) => (
          <React.Fragment key={shortid.generate()}>
            <Dia
              mesSelecionado={mesSelecionado}
              dia={dia}
              eventos={
                eventos?.dias && dia.getMonth() === mesSelecionado.numeroMes - 1
                  ? eventos.dias.filter(
                      diaAtual => Number(diaAtual.dia) === Number(dia.getDate())
                    )[0]
                  : []
              }
              selecionado={
                diaSelecionado
                  ? dia.toString() === diaSelecionado.toString()
                  : false
              }
              numeroDia={dia.getDay()}
              onClick={() => onClickDia(dia)}
            />
            {(indice + 1) % 7 === 0 && (
              <DiaCompleto
                diasPermitidos={obterSemanaPeloIndice(indice)}
                dia={diaSelecionado}
                eventos={eventos.dias}
                carregandoDia={carregandoDia}
                permissaoTela={permissaoTela}
                tipoCalendarioId={tipoCalendarioId}
              />
            )}
          </React.Fragment>
        ))}
    </DiasWrapper>
  );
}

Dias.propTypes = {
  mesSelecionado: t.oneOfType([t.any]),
  diaSelecionado: t.oneOfType([t.any]),
  eventos: t.oneOfType([t.any]),
  onClickDia: t.func,
  carregandoDia: t.bool,
  permissaoTela: t.oneOfType([t.any]),
  tipoCalendarioId: t.oneOfType([t.any]),
};

Dias.defaultProps = {
  mesSelecionado: {},
  diaSelecionado: {},
  eventos: [],
  onClickDia: () => {},
  carregandoDia: false,
  permissaoTela: {},
  tipoCalendarioId: null,
};

export default Dias;
