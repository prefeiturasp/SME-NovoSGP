import React, { useEffect, useState } from 'react';
import { Formik, Form } from 'formik';
import { ModalConteudoHtml, RadioGroupButton, Loader } from '~/componentes';
import ServicoCadastroAula from '~/servicos/Paginas/CalendarioProfessor/CadastroAula/ServicoCadastroAula';
import { erros, sucesso } from '~/servicos/alertas';

function ExcluirAula({
  visivel,
  idAula,
  dataAula,
  nomeComponente,
  onFecharModal,
}) {
  const opcoesExcluirRecorrencia = [
    { label: 'Somente o dia', value: 1 },
    { label: 'Bimestre atual', value: 2 },
    { label: 'Todos os bimestres', value: 3 },
  ];

  const [carregando, setCarregando] = useState(false);
  const [recorrencia, setRecorrenciaAula] = useState({
    aulaId: 0,
    existeFrequenciaOuPlanoAula: false,
    quantidadeAulasRecorrentes: 0,
    recorrenciaAula: 1,
  });

  const excluirAula = () => {
    const nome = nomeComponente();
    setCarregando(true);
    ServicoCadastroAula.excluirAula(idAula, recorrencia.recorrenciaAula, nome)
      .then(resposta => {
        sucesso(resposta.data.mensagens[0]);
        onFecharModal();
      })
      .catch(e => erros(e))
      .finally(() => setCarregando(false));
  };

  useEffect(() => {
    if (visivel) {
      setCarregando(true);
      ServicoCadastroAula.obterRecorrenciaPorIdAula(idAula)
        .then(resposta => setRecorrenciaAula(resposta.data))
        .catch(e => erros(e))
        .finally(() => setCarregando(false));
    }
  }, [idAula, visivel]);

  return (
    <ModalConteudoHtml
      key="excluirAula"
      visivel={visivel}
      onConfirmacaoPrincipal={excluirAula}
      onConfirmacaoSecundaria={onFecharModal}
      onClose={() => {}}
      labelBotaoPrincipal="Confirmar"
      labelBotaoSecundario="Cancelar"
      titulo={`Excluir aula - ${dataAula}`}
      closable={false}
    >
      <Loader loading={carregando}>
        <Formik
          enableReinitialize
          initialValues={{ tipoRecorrenciaExclusao: 1 }}
          onSubmit={() => {}}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row justify-content-start">
                <div
                  className="col-sm-12 col-md-12"
                  style={{ paddingTop: '10px' }}
                >
                  <p>{`Essa aula se repete por ${
                    recorrencia.quantidadeAulasRecorrentes
                  }${
                    recorrencia.quantidadeAulasRecorrentes > 1
                      ? ' vezes'
                      : ' vez'
                  } em seu planejamento.${
                    recorrencia.existeFrequenciaOuPlanoAula
                      ? ' Obs: Esta aula ou sua recorrência possui frequência ou plano de aula registrado, ao excluí-la estará excluindo esse registro também'
                      : ''
                  }`}</p>
                  <p>Qual opção de exclusão você deseja realizar?</p>
                </div>
                <div className="col-sm-12 col-md-12 d-block">
                  <RadioGroupButton
                    form={form}
                    id="tipo-recorrencia-exclusao"
                    label="Realizar exclusão"
                    opcoes={opcoesExcluirRecorrencia}
                    name="tipoRecorrenciaExclusao"
                    onChange={e => {
                      setRecorrenciaAula(r => {
                        return { ...r, recorrenciaAula: e.target.value };
                      });
                    }}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
      </Loader>
    </ModalConteudoHtml>
  );
}

export default ExcluirAula;
