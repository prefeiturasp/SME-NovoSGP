import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { DataTable } from '~/componentes';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import Label from '~/componentes/label';
import ModalCadastroAtendimentoClinico from './modalCadastroAtendimentoClinico';

const AtendimentoClinicoTabela = props => {
  const { label, questaoAtual, form } = props;

  const [exibirModal, setExibirModal] = useState(false);

  const onClickNovoDetalhamento = () => {
    setExibirModal(true);
  };

  const onCloseModal = novosDados => {
    setExibirModal(false);

    if (novosDados) {
      const dadosAtuais = form?.values?.[questaoAtual.id]?.length
        ? form?.values?.[questaoAtual.id]
        : [];
      novosDados.id = dadosAtuais.length + 1;
      dadosAtuais.push(novosDados);
      if (form) {
        form.setFieldValue(questaoAtual.id, dadosAtuais);
      }
    }
  };

  const formatarCampoTabela = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = moment(data).format('HH:mm');
    }
    return <span> {dataFormatada}</span>;
  };

  const colunas = [
    {
      title: 'Dia da Semana',
      dataIndex: 'diaSemana',
    },
    {
      title: 'Atendimento/Atividade',
      dataIndex: 'atendimentoAtividade',
    },
    {
      title: 'Local de realização',
      dataIndex: 'localRealizacao',
    },
    {
      title: 'Horário de início',
      dataIndex: 'horarioInicio',
      render: data => formatarCampoTabela(data),
    },
    {
      title: 'Horário de término',
      dataIndex: 'horarioTermino',
      render: data => formatarCampoTabela(data),
    },
    {
      title: 'Ação',
      dataIndex: 'acaoRemover',
      render: (texto, linha) => {
        return <div>BOTAO</div>;
      },
    },
  ];

  return (
    <>
      <ModalCadastroAtendimentoClinico
        onClose={onCloseModal}
        exibirModal={exibirModal}
      />
      <Label text={label} />
      <DataTable
        rowKey="id"
        columns={colunas}
        dataSource={
          form?.values?.[questaoAtual.id]?.length
            ? form?.values?.[questaoAtual.id]
            : []
        }
        pagination={false}
      />
      <Button
        id="btn-novo-detalhamento"
        label="Novo detalhamento"
        icon="plus"
        color={Colors.Azul}
        border
        className="mr-3 mt-2"
        onClick={onClickNovoDetalhamento}
      />
    </>
  );
};

AtendimentoClinicoTabela.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.string,
};

AtendimentoClinicoTabela.defaultProps = {
  label: '',
  questaoAtual: null,
  form: null,
};

export default AtendimentoClinicoTabela;
