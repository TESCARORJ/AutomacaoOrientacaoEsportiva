# Automação de Registro de Personal Trainers

Este projeto em C# automatiza a busca e o registro de profissionais de cidades/estados do Brasil no site "orientacaoesportiva.com" utilizando a API do Google Maps e o Selenium WebDriver.

## Pré-requisitos

- .NET SDK
- ChromeDriver
- Selenium WebDriver
- Newtonsoft.Json

## Configuração

1. **Clone o repositório:**
git clone https://github.com/seu-usuario/seu-repositorio.git
cd seu-repositorio


2. **Instale as dependências:**

No diretório do projeto, execute:
dotnet add package Selenium.WebDriver
dotnet add package Selenium.WebDriver.ChromeDriver
dotnet add package Newtonsoft.Json


3. **Configure o ChromeDriver:**

Baixe o [ChromeDriver](https://sites.google.com/a/chromium.org/chromedriver/downloads) e atualize o caminho no código:
string chromeDriverPath = @"C:\caminho\para\chromedriver";

 
4. **Atualize a chave da API do Google Maps:**

    Substitua `googleMapsApiKey` no código com sua chave da API do Google Maps.

## Execução

Para executar o programa, use o comando:
dotnet run

## Funcionamento

1. O programa faz uma requisição à API do Google Maps para buscar profissionais de cidades/estados do Brasil.
2. Para cada resultado, o programa:
    - Extrai o nome, endereço e telefone.
    - Gera um email fictício.
    - Abre o navegador e navega até "orientacaoesportiva.com".
    - Preenche e submete o formulário de registro e anúncio.

## Observações

- **Segurança:** A chave da API está exposta no código. É recomendável usar variáveis de ambiente ou um serviço seguro para armazenar chaves sensíveis.
- **Manutenção:** O uso de `Task.Delay` pode ser substituído por esperas explícitas do Selenium para maior robustez.
- **Eficiência:** O navegador é aberto e fechado para cada resultado, o que pode ser otimizado.

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).



    
    
