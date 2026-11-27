const botaoPix = document.getElementById("botaoPix");
const botaoCredito = document.getElementById("botaoCredito");
const botaoDebito = document.getElementById("botaoDebito");

const painelPix = document.getElementById("painelPix");
const painelCartao = document.getElementById("painelCartao");
const painelDebito = document.getElementById("painelDebito");

const campoCartao = document.getElementById('numeroCartao');
const campoDebito = document.getElementById('numeroDebito');

const campoValidade = document.getElementById('validade');
const debitoValidade = document.getElementById('validadeDebito');
const campoCvv = document.getElementById('cvv');
const debitoCvv = document.getElementById('cvvDebito');

const botaoCopiar = document.getElementById('botaoCopiar');
const textoOriginal = botaoCopiar.textContent;

const HANDY_API_KEY = "PUB-272gYZW1jSTVYu9w7tgzbql0p4GnBZ";

botaoPix.addEventListener("click", () => {
    botaoPix.classList.add("ativo");
    botaoCredito.classList.remove("ativo");
    botaoDebito.classList.remove("ativo");

    painelPix.classList.remove("oculto");
    painelCartao.classList.add("oculto");
    painelDebito.classList.add("oculto");
});

botaoCredito.addEventListener("click", () => {
    botaoPix.classList.remove("ativo");
    botaoCredito.classList.add("ativo");
    botaoDebito.classList.remove("ativo");

    painelPix.classList.add("oculto");
    painelCartao.classList.remove("oculto");
    painelDebito.classList.add("oculto");
});

botaoDebito.addEventListener("click", () => {
    botaoPix.classList.remove("ativo");
    botaoCredito.classList.remove("ativo");
    botaoDebito.classList.add("ativo");

    painelPix.classList.add("oculto");
    painelCartao.classList.add("oculto");
    painelDebito.classList.remove("oculto");
});

campoCartao.addEventListener('input', () => {
    const digitos = campoCartao.value.replace(/\D/g, '').slice(0, 16);
    campoCartao.value = digitos.replace(/(.{4})/g, '$1 ').trim();
});
campoDebito.addEventListener('input', () => {
    const digitos = campoDebito.value.replace(/\D/g, '').slice(0, 16);
    campoDebito.value = digitos.replace(/(.{4})/g, '$1 ').trim();
});

campoValidade.addEventListener('input', () => {
    let digitos = campoValidade.value.replace(/\D/g, '').slice(0, 4);

    
    if (digitos.length === 1 && digitos > '1') {
        digitos = '0' + digitos;
    }
    
    if (digitos.length >= 2 && parseInt(digitos.slice(0, 2), 10) > 12) {
        digitos = '12' + digitos.slice(2);
    }

    campoValidade.value = digitos.length > 2
        ? digitos.slice(0, 2) + '/' + digitos.slice(2)
        : digitos;
});

debitoValidade.addEventListener('input', () => {
    let digitos = debitoValidade.value.replace(/\D/g, '').slice(0, 4);

    
    if (digitos.length === 1 && digitos > '1') {
        digitos = '0' + digitos;
    }
    
    if (digitos.length >= 2 && parseInt(digitos.slice(0, 2), 10) > 12) {
        digitos = '12' + digitos.slice(2);
    }

    debitoValidade.value = digitos.length > 2
        ? digitos.slice(0, 2) + '/' + digitos.slice(2)
        : digitos;
});

campoCvv.addEventListener('input', () => {
    campoCvv.value = campoCvv.value.replace(/\D/g, '').slice(0, 4);
});

debitoCvv.addEventListener('input', () => {
    debitoCvv.value = debitoCvv.value.replace(/\D/g, '').slice(0, 4);
});

    botaoCopiar.addEventListener('click', async () => {
        const codigo = botaoCopiar.dataset.codigo;

        try {
            await navigator.clipboard.writeText(codigo);
        } catch (erro) {
            
            const temp = document.createElement('textarea');
            temp.value = codigo;
            document.body.appendChild(temp);
            temp.select();
            document.execCommand('copy');
            document.body.removeChild(temp);
        }

        
        botaoCopiar.textContent = 'Código copiado!';
        setTimeout(() => { botaoCopiar.textContent = textoOriginal; }, 2000);
    });


const consultas = {};



async function consultarBandeira(bin) {
    if (bin in consultas) return consultas[bin];

    const resposta = await fetch(`https://data.handyapi.com/bin/${bin}`, {
        headers: { "x-api-key": HANDY_API_KEY }
    });
    if (!resposta.ok) return "";

    const dados = await resposta.json();
    consultas[bin] = dados.Status === "SUCCESS" ? (dados.Scheme || "") : "";
    return consultas[bin];
}

function mostrarBandeira(nome, logo, texto) {
    const procurado = nome.toLowerCase();
    const bandeira = Object.entries(bandeiras).find(([chave, dados]) =>
        chave.toLowerCase() === procurado ||
        dados.alias?.toLowerCase() === procurado
    );

    if (bandeira) {
        logo.src = bandeira[1].image;
        logo.alt = bandeira[0];
        logo.classList.remove("oculto");
        texto.textContent = bandeira[0];
    } else {
        logo.classList.add("oculto");
        texto.textContent = nome;
    }
}

function configurarCartao(idInput, idLogo, idTexto) {
    const input = document.getElementById(idInput);
    const logo = document.getElementById(idLogo);
    const texto = document.getElementById(idTexto);
    if (!input || !logo || !texto) return;

    input.addEventListener("input", async () => {
        const bin = input.value.replace(/\D/g, "").slice(0, 6);
        let nome = "";

        if (bin.length === 6) {
            try {
                nome = await consultarBandeira(bin);
            } catch (erro) {
                console.log("Erro na consulta da bandeira", erro);
            }
        }

        // ignora resposta antiga se o número mudou durante a consulta
        if (input.value.replace(/\D/g, "").slice(0, 6) !== bin) return;
        mostrarBandeira(nome, logo, texto);
    });
}


configurarCartao("numeroCartao", "logoCredito", "bandeiraCredito");
configurarCartao("numeroDebito", "logoDebito", "bandeiraDebito");