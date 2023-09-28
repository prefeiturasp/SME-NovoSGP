pipeline {
    environment {
      branchname =  env.BRANCH_NAME.toLowerCase()
      kubeconfig = getKubeconf(env.branchname)
      registryCredential = 'jenkins_registry'
      deployment1 = "${env.branchname == 'release-r2' ? 'sme-api-rc2' : 'sme-api' }"           
      deployment2 = "${env.branchname == 'release-r2' ? 'sme-worker-fechamento-r2' : 'sme-worker-fechamento' }"  
      deployment3 = "${env.branchname == 'release-r2' ? 'sme-worker-geral-r2' : 'sme-worker-geral' }"
      deployment4 = "${env.branchname == 'release-r2' ? 'sme-worker-aee-r2' : 'sme-worker-aee' }"
      deployment5 = "${env.branchname == 'release-r2' ? 'sme-worker-aula-r2' : 'sme-worker-aula' }"
      deployment6 = "${env.branchname == 'release-r2' ? 'sme-worker-frequencia-r2' : 'sme-worker-frequencia' }"
      deployment7 = "${env.branchname == 'release-r2' ? 'sme-worker-institucional-r2' : 'sme-worker-institucional' }"
      deployment8 = "${env.branchname == 'release-r2' ? 'sme-worker-pendencias-r2' : 'sme-worker-pendencias' }"
      deployment9 = "${env.branchname == 'release-r2' ? 'sme-worker-avaliacao-r2' : 'sme-worker-avaliacao' }"
      deployment10 = "${env.branchname == 'release-r2' ? 'sme-worker-auditoria-r2' : 'sme-worker-auditoria' }"
      deployment11 = "${env.branchname == 'release-r2' ? 'sme-worker-notificacoes-r2' : 'sme-worker-notificacoes' }"
      deployment12 = "${env.branchname == 'release-r2' ? 'sme-worker-notificacoes-hub-r2' : 'sme-worker-notificacoes-hub' }"
      deployment13 = "${env.branchname == 'release-r2' ? 'sme-worker-compressao-r2' : 'sme-worker-compressao' }"
      deployment14 = "${env.branchname == 'release-r2' ? 'sme-worker-naapa-r2' : 'sme-worker-naapa' }"
      namespace = "${env.branchname == 'pre-prod' ? 'sme-novosgp-d1' : env.branchname == 'development' ? 'novosgp-dev' : 'sme-novosgp' }"
           
    }

    agent {
      kubernetes { label 'builder' }
    }

    options {
      timestamps ()
      buildDiscarder(logRotator(numToKeepStr: '20', artifactNumToKeepStr: '20'))
      disableConcurrentBuilds()
      skipDefaultCheckout()
    }
  
    stages {

        stage('CheckOut') {            
            steps { checkout scm }            
        }
   
        stage('Build') {
          when { anyOf { branch 'master'; branch 'main'; branch 'pre-prod'; branch "story/*"; branch 'development'; branch 'release'; branch 'release-r2'; branch 'infra/*'; } } 
          parallel {
            stage('sme-sgp-backend') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-sgp-backend"
                  dockerImage1 = docker.build(imagename, "-f src/SME.SGP.Api/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage1.push() } 
                }
              }
            }
            stage('sme-worker-geral') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-geral"
                  dockerImage2 = docker.build(imagename, "-f src/SME.SGP.Worker.Rabbbit/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage2.push() }   
                }
              }
            }
            stage('sme-worker-fechamento') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-fechamento"
                  dockerImage3 = docker.build(imagename, "-f src/SME.SGP.Fechamento.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage3.push() }   
                }
              }
            }
            stage('sme-worker-aee') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-aee"
                  dockerImage4 = docker.build(imagename, "-f src/SME.SGP.AEE.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage4.push() }  
                }
              }
            }
            stage('sme-worker-aula') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-aula"
                  dockerImage5 = docker.build(imagename, "-f src/SME.SGP.Aula.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage5.push() }   
                }
              }
            }
            stage('sme-worker-frequencia') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-frequencia"
                  dockerImage6 = docker.build(imagename, "-f src/SME.SGP.Frequencia.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage6.push() }   
                }
              }
            }
            stage('sme-worker-institucional') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-institucional"
                  dockerImage7 = docker.build(imagename, "-f src/SME.SGP.Institucional.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage7.push() }   
                }
              }
            }
            stage('sme-worker-pendencias') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-pendencias"
                  dockerImage8 = docker.build(imagename, "-f src/SME.SGP.Pendencias.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage8.push() }  
                }
              }
            }
            stage('sme-worker-avaliacao') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-avaliacao"
                  dockerImage9 = docker.build(imagename, "-f src/SME.SGP.Avaliacao.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage9.push() }  
                }
              }
            }
            stage('sme-worker-auditoria') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-auditoria"
                  dockerImage10 = docker.build(imagename, "-f src/SME.SGP.Auditoria.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage10.push() }  
                }
              }
            }
            stage('sme-worker-notificacoes') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-notificacoes"
                  dockerImage11 = docker.build(imagename, "-f src/SME.SGP.Notificacoes.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage11.push() }  
                }
              }
            }
            stage('sme-worker-notificacoes-hub') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-notificacoes-hub"
                  dockerImage12 = docker.build(imagename, "-f src/SME.SGP.Notificacoes.Hub/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage12.push() }   
                }
              }
            }            
            stage('sme-worker-compressao') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-compressao"
                  dockerImage13 = docker.build(imagename, "-f src/SME.SGP.ComprimirArquivos.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage13.push() }  
                }
              }
            }
            stage('sme-worker-naapa') {
              agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
              steps{
                checkout scm
                script {
                  imagename = "registry.sme.prefeitura.sp.gov.br/${env.branchname}/sme-worker-naapa"
                  dockerImage14 = docker.build(imagename, "-f src/SME.SGP.NAAPA.Worker/Dockerfile .")
                  docker.withRegistry( 'https://registry.sme.prefeitura.sp.gov.br', registryCredential ) {
                  dockerImage14.push() }  
                }
              }
            }   
          }
    }
        stage('Deploy'){
             agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
            when { anyOf {  branch 'master'; branch 'main'; branch 'development'; branch 'release'; branch 'release-r2'; branch 'pre-prod'; } }        
            steps {
                script{
                  //if(testPassed){
                        if ( env.branchname == 'main' ||  env.branchname == 'master' ) {
                            withCredentials([string(credentialsId: 'aprovadores-sgp', variable: 'aprovadores')]) {
                                timeout(time: 24, unit: "HOURS") {
                                    input message: 'Deseja realizar o deploy?', ok: 'SIM', submitter: "${aprovadores}"
                                }
                            }
                        }
                        withCredentials([file(credentialsId: "${kubeconfig}", variable: 'config')]){
                                sh('rm -f '+"$home"+'/.kube/config')
                                sh('cp $config '+"$home"+'/.kube/config')
                                sh "kubectl rollout restart deployment/${deployment1} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment2} -n ${namespace}"   
                                sh "kubectl rollout restart deployment/${deployment3} -n ${namespace}"   
                                sh "kubectl rollout restart deployment/${deployment4} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment5} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment6} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment7} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment8} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment9} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment10} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment11} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment12} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment13} -n ${namespace}"
                                sh "kubectl rollout restart deployment/${deployment14} -n ${namespace}"
                                sh('rm -f '+"$home"+'/.kube/config')
                        }
                    //}
                }
            }           
        }
             
      stage('Flyway') {
        agent { kubernetes { 
                  label 'flyway'
                  defaultContainer 'flyway'
                }
              }
        when { anyOf {  branch 'master'; branch 'main'; branch 'development'; branch 'release'; branch 'release-r2'; branch 'pre-prod'; } }
        steps{
          withCredentials([string(credentialsId: "flyway_sgp_${branchname}", variable: 'url')]) {
            checkout scm
            sh 'pwd'
            sh 'ls'
            sh 'flyway -url=$url -locations="filesystem:scripts" -outOfOrder=true migrate'
             }
        }       
      }

      stage('Deploy Treinamento'){
          agent { kubernetes { 
                  label 'builder'
                  defaultContainer 'builder'
                }
              }
          when { anyOf { branch 'release'; } }        
          steps {
              script{
                  try {
                      withCredentials([file(credentialsId: "${kubeconfig}", variable: 'config')]){
                          sh('rm -f '+"$home"+'/.kube/config')
                          sh('cp $config '+"$home"+'/.kube/config')
                          sh "kubectl -n sme-novosgp-treino rollout restart deploy"
                          sh('rm -f '+"$home"+'/.kube/config')
                      }
                  }
                  catch (err) {
                      echo err.getMessage()
                  }
              }
          }           
      }

      stage('Treinamento Flyway') {
        agent { kubernetes { 
                  label 'flyway'
                  defaultContainer 'flyway'
                }
              }
        when { anyOf {  branch 'release'; } }
        steps{
          script{
            try {
                withCredentials([string(credentialsId: "flyway_sgp_treinamento", variable: 'url')]) {
                checkout scm
                sh 'flyway -url=$url -locations="filesystem:scripts" -outOfOrder=true migrate'
                }
            } 
            catch (err) {
                echo err.getMessage()
            }
          }
        }       
      }        
    }

  post {
    success { sendTelegram("🚀 Job Name: ${JOB_NAME} \nBuild: ${BUILD_DISPLAY_NAME} \nStatus: Success \nLog: \n${env.BUILD_URL}console") }
    unstable { sendTelegram("💣 Job Name: ${JOB_NAME} \nBuild: ${BUILD_DISPLAY_NAME} \nStatus: Unstable \nLog: \n${env.BUILD_URL}console") }
    failure { sendTelegram("💥 Job Name: ${JOB_NAME} \nBuild: ${BUILD_DISPLAY_NAME} \nStatus: Failure \nLog: \n${env.BUILD_URL}console") }
    aborted { sendTelegram ("😥 Job Name: ${JOB_NAME} \nBuild: ${BUILD_DISPLAY_NAME} \nStatus: Aborted \nLog: \n${env.BUILD_URL}console") }
  }
}
def sendTelegram(message) {
    def encodedMessage = URLEncoder.encode(message, "UTF-8")
    withCredentials([string(credentialsId: 'telegramToken', variable: 'TOKEN'),
    string(credentialsId: 'telegramChatId', variable: 'CHAT_ID')]) {
        response = httpRequest (consoleLogResponseBody: true,
                contentType: 'APPLICATION_JSON',
                httpMode: 'GET',
                url: 'https://api.telegram.org/bot'+"$TOKEN"+'/sendMessage?text='+encodedMessage+'&chat_id='+"$CHAT_ID"+'&disable_web_page_preview=true',
                validResponseCodes: '200')
        return response
    }
}
def getKubeconf(branchName) {
    if("main".equals(branchName)) { return "config_prd"; }
    else if ("master".equals(branchName)) { return "config_prd"; }
    else if ("pre-prod".equals(branchName)) { return "config_prd"; }
    else if ("homolog".equals(branchName)) { return "config_hom"; }
    else if ("release".equals(branchName)) { return "config_hom"; }
    else if ("release-r2".equals(branchName)) { return "config_hom"; }
    else if ("development".equals(branchName)) { return "config_release"; }
    else if ("develop".equals(branchName)) { return "config_release"; }
}
