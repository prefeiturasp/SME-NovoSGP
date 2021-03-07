import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { ListaPaginada } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';

const RegistroComunicacaoEscolaAquiCardCollapse = props => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const [exibir, setExibir] = useState(false);

  const colunas = [
    {
      title: 'Data',
      dataIndex: 'dataEnvio',
      render: data => {
        let dataFormatada = '';
        if (data) {
          dataFormatada = moment(data).format('DD/MM/YYYY');
        }
        return <span> {dataFormatada}</span>;
      },
    },
    {
      title: 'Categoria',
      dataIndex: 'categoriaNome',
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
    },
    {
      title: 'Leitura',
      dataIndex: 'statusLeitura',
    },
  ];

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="registro-comunicacao-escola-aqui-acompanhamento-aprendizagem-collapse"
        onClick={onClickExpandir}
        titulo="Registro de comunicação (Escola aqui)"
        indice="registro-comunicacao-escola-aqui-acompanhamento-aprendizagem"
        show={exibir}
        alt="registro-comunicacao-escola-aqui-acompanhamento-aprendizagem"
      >
        <div className="col-md-12 mb-2">
          {turmaSelecionada?.id && codigoEOL && semestreSelecionado ? (
            <ListaPaginada
              url={`v1/comunicado/turmas/${turmaSelecionada?.id}/semestres/${semestreSelecionado}/alunos/${codigoEOL}`}
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
        </div>
      </CardCollapse>
    </div>
  );
};

RegistroComunicacaoEscolaAquiCardCollapse.propTypes = {
  semestreSelecionado: PropTypes.string,
};

RegistroComunicacaoEscolaAquiCardCollapse.defaultProps = {
  semestreSelecionado: '',
};

export default RegistroComunicacaoEscolaAquiCardCollapse;
