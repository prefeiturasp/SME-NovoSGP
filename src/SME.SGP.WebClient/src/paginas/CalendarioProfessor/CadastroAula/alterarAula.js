import React, { useEffect, useState } from 'react';
import { Formik, Form } from 'formik';
import { ModalConteudoHtml, RadioGroupButton, Loader } from '~/componentes';

function AlterarAula({
    visivel,
    dataAula,
    nomeComponente,
    recorrencia,
    recorrenciaSelecionada,
    onFecharModal,
    onCancelar,
}) {

    const tipoRecorrencia = {
        AULA_UNICA: 1,
        REPETIR_BIMESTRE_ATUAL: 2,
        REPETIR_TODOS_BIMESTRES: 3,
    };

    const [carregando, setCarregando] = useState(false);

    const alterarAula = () => {
        const nome = nomeComponente();
        setCarregando(true);
        onFecharModal(true);
    };

    useEffect(() => {
        if (visivel) {

        }
    }, [visivel]);

    return (
        <ModalConteudoHtml
            key="alterarAula"
            visivel={visivel}
            onConfirmacaoPrincipal={alterarAula}
            onConfirmacaoSecundaria={onCancelar}
            onClose={onCancelar}
            labelBotaoPrincipal="Confirmar"
            labelBotaoSecundario="Cancelar"
            titulo={`Alterar aula - ${dataAula}`}
            closable={false}
        >
            <Loader loading={carregando}>
                <Formik
                    enableReinitialize
                    initialValues={{ tipoRecorrenciaAlteracao: 1 }}
                    onSubmit={() => { }}
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
                                    <p>{recorrenciaSelecionada === tipoRecorrencia.AULA_UNICA ?
                                        `Esta aula já possui registro de frequência, após a alteração da aula, sugerimos que você confira a frequência alterada.` :
                                        `Essa aula se repete por ${
                                        recorrencia.quantidadeAulasRecorrentes
                                        }${
                                        recorrencia.quantidadeAulasRecorrentes > 1
                                            ? ' vezes'
                                            : ' vez'
                                        } no seu calendário.${
                                        recorrencia.existeFrequenciaOuPlanoAula
                                            ? ' OBS.: Esta aula ou alguma da sua recorrência já possui registro de frequência, após a alteração da aula, sugerimos que você confira a frequência alterada.'
                                            : ''
                                        }`}</p>
                                    <p>Confirma alteração nas aulas?</p>
                                </div>
                            </div>
                        </Form>
                    )}
                </Formik>
            </Loader>
        </ModalConteudoHtml>
    );
}

export default AlterarAula;