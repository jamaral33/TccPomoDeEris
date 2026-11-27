const dataFinal = new Date("2026-12-31T23:59:59").getTime();

function atualizarContagem() {
    const agora = new Date().getTime();
    const tempoRestante = dataFinal - agora;

    if (tempoRestante <= 0) {
        clearInterval(contagem);

        document.getElementById("cd-days").textContent = "00";
        document.getElementById("cd-hours").textContent = "00";
        document.getElementById("cd-min").textContent = "00";
        document.getElementById("cd-sec").textContent = "00";

        document.getElementById("countdown").innerHTML = "Promoção encerrada!";
        return;
    }

    const dias = Math.floor(tempoRestante / (1000 * 60 * 60 * 24));
    const horas = Math.floor((tempoRestante / (1000 * 60 * 60)) % 24);
    const minutos = Math.floor((tempoRestante / (1000 * 60)) % 60);
    const segundos = Math.floor((tempoRestante / 1000) % 60);

    document.getElementById("cd-days").textContent = dias;
    document.getElementById("cd-hours").textContent = String(horas).padStart(2, "0");
    document.getElementById("cd-min").textContent = String(minutos).padStart(2, "0");
    document.getElementById("cd-sec").textContent = String(segundos).padStart(2, "0");
}

atualizarContagem();

const contagem = setInterval(atualizarContagem, 1000);