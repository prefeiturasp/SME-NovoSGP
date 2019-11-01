import React, { useState } from 'react';
import PropTypes from 'prop-types';

// Ant

// Styles
import { ContainerModal, VerticalCentered, IconMargin } from './styles';

// Components
import { CampoData } from '~/componentes/campoData/campoData';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import BootstrapRow from './components/BootstrapRow';
import WeekDays from './components/WeekDays';
import MonthlyRecurrence from './components/MonthlyRecurrence';
import DropDownQuantidade from './components/DropDownQuantidade';
import DropDownTipoRecorrencia from './components/DropDownTipoRecorrencia';

function ModalRecorrencia({ show, onCloseRepetir }) {
  const [dataInicio, setDataInicio] = useState('');
  const [dataTermino, setDataTermino] = useState('');
  const [diasSemana, setDiasSemana] = useState([]);
  const [quantidadeRecorrencia, setQuantidadeRecorrencia] = useState(0);
  const [tipoRecorrencia, setTipoRecorrencia] = useState({});

  const onChangeWeekDay = day => {
    const exists = diasSemana.some(x => x.value === day.value);
    if (exists) {
      setDiasSemana([...diasSemana.filter(x => x.value !== day.value)]);
    } else {
      setDiasSemana([...diasSemana, day]);
    }
  };

  /**
   * @description Render helper text above WeekDays component
   */
  const renderHelperText = () => {
    let text = `Ocorre a cada `;
    if (diasSemana.length === 1) {
      text += diasSemana[0].value;
    } else if (diasSemana.length === 2) {
      text += `${diasSemana[0].value} e ${diasSemana[1].value}`;
    } else if (diasSemana.length > 2) {
      text += `${diasSemana
        .map((item, index) =>
          index !== diasSemana.length - 1 ? item.value : ''
        )
        .toString()} e ${diasSemana[diasSemana.length - 1].value}.`;
    }
    return text;
  };

  const onCloseModal = () => {
    setDataInicio('');
    setDataTermino('');
    setDiasSemana([]);
    onCloseRepetir();
  };

  const onChangeDataInicio = e => setDataInicio(e);
  const onChangeDataTermino = e => setDataTermino(e);

  return (
    <ContainerModal>
      <ModalConteudoHtml
        visivel={show}
        closable={!!true}
        onClose={() => onCloseModal()}
        titulo="Repetir"
        labelBotaoPrincipal="Salvar"
        labelBotaoSecundario="Descartar"
      >
        <BootstrapRow paddingBottom={4}>
          <VerticalCentered className="col-lg-6">
            <CampoData
              label="Data início"
              valor={dataInicio}
              onChange={onChangeDataInicio}
              placeholder="DD/MM/AAAA"
            />
          </VerticalCentered>
          <VerticalCentered className="col-lg-6">
            <CampoData
              label="Data fim"
              valor={dataTermino}
              onChange={onChangeDataTermino}
              placeholder="DD/MM/AAAA"
            />
          </VerticalCentered>
        </BootstrapRow>
        <BootstrapRow paddingBottom={3}>
          <VerticalCentered className="col-lg-4">
            <IconMargin className="fas fa-retweet" />
            <span>Repetir a cada</span>
          </VerticalCentered>
          <VerticalCentered className="col-lg-2">
            <DropDownQuantidade
              onChange={value => setQuantidadeRecorrencia(value)}
              value={quantidadeRecorrencia}
            />
          </VerticalCentered>
          <VerticalCentered className="col-lg-4">
            <DropDownTipoRecorrencia
              onChange={value => setTipoRecorrencia(value)}
              value={tipoRecorrencia}
            />
          </VerticalCentered>
        </BootstrapRow>
        <BootstrapRow paddingBottom={3}>
          <VerticalCentered className="col-lg-12">
            {tipoRecorrencia && tipoRecorrencia.value === 'S' ? (
              <WeekDays
                currentState={diasSemana || null}
                onChange={e => onChangeWeekDay(e)}
              />
            ) : (
              <MonthlyRecurrence />
            )}
          </VerticalCentered>
        </BootstrapRow>
        {diasSemana.length > 0 && (
          <BootstrapRow>
            <VerticalCentered className="col-lg-12">
              {renderHelperText()}
            </VerticalCentered>
          </BootstrapRow>
        )}
      </ModalConteudoHtml>
    </ContainerModal>
  );
}

ModalRecorrencia.defaultProps = {
  show: false,
  onCloseRepetir: () => {},
};

ModalRecorrencia.propTypes = {
  show: PropTypes.bool,
  onCloseRepetir: PropTypes.func,
};

export default ModalRecorrencia;
