import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Dashboard/ServicoDashboardEscolaAqui';
import { ContainerDataUltimaAtualizacao } from '../../dashboardEscolaAqui.css';

const DataUltimaAtualizacaoDashboardEscolaAqui = props => {
  const { tituloAdicional, nomeConsulta } = props;

  const [ultimaAtualizacao, setUltimaAtualizacao] = useState();

  const obterDataUltimaAtualizacao = async () => {
    const retorno = await ServicoDashboardEscolaAqui.obterUltimaAtualizacaoPorProcesso(
      nomeConsulta
    );
    if (retorno && retorno.data && retorno.data.dataUltimaAtualizacao) {
      setUltimaAtualizacao(
        moment(retorno.data.dataUltimaAtualizacao).format('DD/MM/YYYY HH:mm')
      );
    }
  };

  useEffect(() => {
    obterDataUltimaAtualizacao();
  }, []);

  return ultimaAtualizacao ? (
    <div className="d-flex justify-content-end pb-4">
      <ContainerDataUltimaAtualizacao>
        <div>Data da última atualização: {ultimaAtualizacao}</div>
        {tituloAdicional ? <div>{tituloAdicional}</div> : ''}
      </ContainerDataUltimaAtualizacao>
    </div>
  ) : (
    ''
  );
};

DataUltimaAtualizacaoDashboardEscolaAqui.propTypes = {
  tituloAdicional: PropTypes.string,
  nomeConsulta: PropTypes.string,
};

DataUltimaAtualizacaoDashboardEscolaAqui.defaultProps = {
  tituloAdicional: '',
  nomeConsulta: '',
};

export default DataUltimaAtualizacaoDashboardEscolaAqui;
