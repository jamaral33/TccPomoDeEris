document.addEventListener('DOMContentLoaded', () => {
  const form = document.getElementById('login-form');
  const emailInput = document.getElementById('email');
  const passwordInput = document.getElementById('password');
  const emailError = document.getElementById('email-error');
  const passwordError = document.getElementById('password-error');
  const togglePassword = document.getElementById('toggle-password');
  const submitBtn = document.getElementById('submit-btn');
  const googleBtn = document.getElementById('google-btn');
  const createAccountBtn = document.getElementById('create-account-btn');
  const forgotLink = document.getElementById('forgot-link');
  const toast = document.getElementById('toast');

  function showToast(message, duration = 2600) {
    toast.textContent = message;
    toast.classList.add('show');
    clearTimeout(showToast._t);
    showToast._t = setTimeout(() => toast.classList.remove('show'), duration);
  }

  function setError(input, errorEl, message) {
    const control = input.closest('.field__control');
    if (message) {
      control.classList.add('has-error');
      errorEl.textContent = message;
    } else {
      control.classList.remove('has-error');
      errorEl.textContent = '';
    }
  }

  function isValidEmail(value) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
  }

  // Mostrar/ocultar senha
  togglePassword.addEventListener('click', () => {
    const isPassword = passwordInput.type === 'password';
    passwordInput.type = isPassword ? 'text' : 'password';
    togglePassword.setAttribute('aria-label', isPassword ? 'Ocultar senha' : 'Mostrar senha');
  });

  // Validação em tempo real
  emailInput.addEventListener('input', () => {
    if (emailInput.value && !isValidEmail(emailInput.value)) {
      setError(emailInput, emailError, 'Digite um e-mail válido.');
    } else {
      setError(emailInput, emailError, '');
    }
  });

  passwordInput.addEventListener('input', () => {
    if (passwordInput.value && passwordInput.value.length < 6) {
      setError(passwordInput, passwordError, 'A senha deve ter ao menos 6 caracteres.');
    } else {
      setError(passwordInput, passwordError, '');
    }
  });

  // Envio do formulário
  form.addEventListener('submit', (event) => {
    event.preventDefault();
    let valid = true;

    if (!emailInput.value || !isValidEmail(emailInput.value)) {
      setError(emailInput, emailError, 'Digite um e-mail válido.');
      valid = false;
    }

    if (!passwordInput.value || passwordInput.value.length < 6) {
      setError(passwordInput, passwordError, 'A senha deve ter ao menos 6 caracteres.');
      valid = false;
    }

    if (!valid) return;

    submitBtn.disabled = true;
    submitBtn.textContent = 'Entrando...';

    // Simulação de chamada de login (substituir por integração real)
    setTimeout(() => {
      submitBtn.disabled = false;
      submitBtn.textContent = 'Entrar';
      showToast('Login realizado com sucesso!');
      // window.location.href = '../home/index.html';
    }, 1200);
  });

  googleBtn.addEventListener('click', () => {
    showToast('Login com Google ainda não configurado.');
  });

  createAccountBtn.addEventListener('click', () => {
    showToast('Redirecionando para criação de conta...');
  });

  forgotLink.addEventListener('click', (event) => {
    event.preventDefault();
    showToast('Enviaremos um link de recuperação para o seu e-mail.');
  });
});
