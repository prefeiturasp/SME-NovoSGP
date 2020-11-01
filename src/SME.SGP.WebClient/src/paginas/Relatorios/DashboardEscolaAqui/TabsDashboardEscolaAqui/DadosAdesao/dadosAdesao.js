import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Graficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';

const DadosAdesao = props => {
  const { codigoDre, codigoUe } = props;

  const [dadosGraficoAdesao, setDadosGraficoAdesao] = useState([]);
  const [exibirLoader, setExibirLoader] = useState([]);

  const obterDadosGraficoAdesao = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardEscolaAqui.obterDadosGraficoAdesao(
      codigoDre,
      codigoUe
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno && retorno.data && retorno.data.length) {
      setDadosGraficoAdesao(retorno.data);
    }
  }, [codigoDre, codigoUe]);

  useEffect(() => {
    if (codigoDre && codigoUe) {
      obterDadosGraficoAdesao();
    } else {
      setDadosGraficoAdesao([]);
    }
  }, [codigoDre, codigoUe, obterDadosGraficoAdesao]);

  return (
    <Loader loading={exibirLoader}>
      <div style={{ height: 400 }}>
        <Graficos.Pie data={dadosGraficoAdesao} />
      </div>
    </Loader>
  );
};

DadosAdesao.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
};

DadosAdesao.defaultProps = {
  codigoDre: '',
  codigoUe: '',
};

export default DadosAdesao;
