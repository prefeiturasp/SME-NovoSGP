import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { Graficos, Loader } from '~/componentes';
import { erros } from '~/servicos';
import ServicoDashboardEscolaAqui from '~/servicos/Paginas/Relatorios/EscolaAqui/DashboardEscolaAqui/ServicoDashboardEscolaAqui';

const DadosAdesao = props => {
  const { codigoDre, codigoUe } = props;

  const [dadosGraficoAdesao, setDadosGraficoAdesao] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);
  const [dataUltimaAtualizacao, setDataUltimaAtualizacao] = useState();

  const obterDataUltimaAtualizacao = async () => {
    const retorno = await ServicoDashboardEscolaAqui.obterUltimaAtualizacaoPorProcesso(
      'ConsolidarAdesaoEOL'
    );
    if (retorno && retorno.data && retorno.data.dataUltimaAtualizacao) {
      setDataUltimaAtualizacao(
        moment(retorno.data.dataUltimaAtualizacao).format('DD/MM/YYYY HH:mm')
      );
    }
  };

  const mapearParaDtoGraficoPizza = dados => {
    const totalUsuariosComCpfInvalidos = {
      label: 'Responsáveis sem CPF ou com CPF inválido no EOL',
      value: dados.totalUsuariosComCpfInvalidos || 0,
      color: '#F98F84',
    };

    const totalUsuariosSemAppInstalado = {
      label: 'Usuários que não realizaram a instalação',
      value: dados.totalUsuariosSemAppInstalado || 0,
      color: '#57CDBC',
    };

    const totalUsuariosPrimeiroAcesso = {
      label: 'Usuários com primeiro acesso incompleto',
      value: dados.totalUsuariosPrimeiroAcesso || 0,
      color: '#EFB971',
    };

    const totalUsuariosValidos = {
      label: 'Usuários válidos',
      value: dados.totalUsuariosValidos || 0,
      color: '#3982AC',
    };

    const dadosMapeados = [
      totalUsuariosComCpfInvalidos,
      totalUsuariosSemAppInstalado,
      totalUsuariosPrimeiroAcesso,
      totalUsuariosValidos,
    ];

    setDadosGraficoAdesao(dadosMapeados);
  };

  const obterDadosGraficoAdesao = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardEscolaAqui.obterDadosGraficoAdesao(
      codigoDre === '-99' ? '' : codigoDre,
      codigoUe === '-99' ? '' : codigoUe
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno && retorno.data && retorno.data.length) {
      mapearParaDtoGraficoPizza(retorno.data[0]);
    } else {
      setDadosGraficoAdesao([]);
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
