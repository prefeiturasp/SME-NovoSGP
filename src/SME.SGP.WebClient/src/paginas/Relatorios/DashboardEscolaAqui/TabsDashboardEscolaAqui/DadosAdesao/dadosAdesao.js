import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Graficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';

const DadosAdesao = props => {
  const { codigoDre, codigoUe } = props;

  const [dadosGraficoAdesao, setDadosGraficoAdesao] = useState([]);
  const [exibirLoader, setExibirLoader] = useState([]);
  const [dataUltimaAtualizacao, setDataUltimaAtualizacao] = useState();

  const obterDataUltimaAtualizacao = async () => {
    const retorno = await ServicoDashboardEscolaAqui.obterUltimaAtualizacaoPorProcesso();
    if (retorno && retorno.data) {
      setDataUltimaAtualizacao(moment(retorno.data).format('DD/MM/YYYY HH:mm'));
    }
  };

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

    obterDataUltimaAtualizacao();
  }, [codigoDre, codigoUe, obterDadosGraficoAdesao]);

  return (
    <Loader loading={exibirLoader} className="text-center">
      {dadosGraficoAdesao && dadosGraficoAdesao.length ? (
        <>
          {dataUltimaAtualizacao ? (
            <div className="col-md-12" style={{ textAlign: 'end' }}>
              Data da última atualização: {dataUltimaAtualizacao}
            </div>
          ) : (
            ''
          )}
          <div
            className="col-md-12"
            style={{
              fontSize: '24px',
              fontWeight: 700,
              textAlign: 'center',
              color: '#000000',
            }}
          >
            Total de Usuários
          </div>
          <div className="col-md-12" style={{ height: 400 }}>
            <Graficos.Pie data={dadosGraficoAdesao} />
          </div>
        </>
      ) : (
        ''
      )}
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
