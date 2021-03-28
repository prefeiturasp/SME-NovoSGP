import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { ListaPaginada } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';

const OcorrenciasCardCollapse = props => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const [exibir, setExibir] = useState(true);

  const colunas = [
    {
      title: 'Data',
      dataIndex: 'dataOcorrencia',
      render: data => {
        let dataFormatada = '';
        if (data) {
          dataFormatada = moment(data).format('DD/MM/YYYY');
        }
        return <span> {dataFormatada}</span>;
      },
    },
    {
      title: 'Registrado por',
      dataIndex: 'registradoPor',
    },
    {
      title: 'Título da ocorrência',
      dataIndex: 'titulo',
    },
  ];

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="ocorrencias-acompanhamento-aprendizagem-collapse"
        onClick={onClickExpandir}
        titulo="Ocorrências"
        indice="ocorrencias-acompanhamento-aprendizagem"
        show={exibir}
        alt="ocorrencias-acompanhamento-aprendizagem"
      >
        {turmaSelecionada?.id && codigoEOL && semestreSelecionado ? (
          <ListaPaginada
            url={`v1/ocorrencias/turma/${turmaSelecionada?.id}/aluno/${codigoEOL}/semestre/${semestreSelecionado}`}
            id="lista-ocorrencias-acompanhamento-aprendizagem"
            colunas={colunas}
            filtro={{}}
            filtroEhValido={
              !!(
                exibir &&
                turmaSelecionada?.id &&
                codigoEOL &&
                semestreSelecionado
              )
            }
            showSizeChanger={false}
          />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

OcorrenciasCardCollapse.propTypes = {
  semestreSelecionado: PropTypes.string,
};

OcorrenciasCardCollapse.defaultProps = {
  semestreSelecionado: '',
};

export default OcorrenciasCardCollapse;
