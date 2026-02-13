<template>
  <div class="page">
    <div class="card">
      <div class="brand">
        <div class="brand-mark"></div>
        <div class="brand-text">
          <h1>Bem-vindo</h1>
          <p>Faça login para continuar</p>
        </div>
      </div>

      <form class="form" @submit.prevent="authenticate">
        <label class="field">
          <span>Usuário</span>
          <input
            v-model="data.username"
            type="text"
            name="username"
            autocomplete="username"
            placeholder="Digite seu usuário"
            required
          />
        </label>

        <label class="field">
          <span>Senha</span>
          <input
            v-model="data.password"
            type="password"
            name="password"
            autocomplete="current-password"
            placeholder="Digite sua senha"
            required
          />
        </label>

        <div class="actions">
          <button class="submit" type="submit" :disabled="isLoading">
            <span v-if="isLoading">Carregando...</span>
            <span v-else>Entrar</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from "vue";
import LoginService from "@/services/LoginService";

let _loginService = new LoginService();
let isLoading = ref(false);

const data = reactive({
  username: "",
  password: "",
});

async function authenticate() {
  if (isLoading.value) return;
  if (!data.username || !data.password) { // se algum campo estiver vazio
    alert("Preencha usuário e senha.");
    return;
  }

  isLoading.value = true; // Marca que a req começou desabilita o botao
  try {
    const response = await _loginService.Login(data.username, data.password); // chama o login pra API
    alert(response?.token ?? "Login OK");
  } catch (error) {
    alert("Erro ao login."); // se tiver off a api da erro
  } finally { // desliga o loading
    isLoading.value = false;
  }
}


</script>

<style scoped>
:global(body) {
  margin: 0;
  font-family: "Poppins", "Segoe UI", sans-serif;
  background: linear-gradient(135deg, #eef3f1 0%, #e3edf5 50%, #d7e7e3 100%);
  color: #1f2a2e;
}

.page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
}

.card {
  width: 100%;
  max-width: 420px;
  background: rgba(255, 255, 255, 0.9);
  border-radius: 20px;
  padding: 32px;
  box-shadow: 0 30px 60px rgba(31, 42, 46, 0.12);
  backdrop-filter: blur(10px);
}

.brand {
  display: flex;
  gap: 16px;
  align-items: center;
  margin-bottom: 24px;
}

.brand-mark {
  width: 48px;
  height: 48px;
  border-radius: 16px;
  background: linear-gradient(145deg, #7fbda5, #a8d0bd);
  box-shadow: inset 0 0 10px rgba(255, 255, 255, 0.6);
}

.brand-text h1 {
  margin: 0;
  font-size: 1.6rem;
}

.brand-text p {
  margin: 4px 0 0;
  color: #5b6b6f;
  font-size: 0.95rem;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 8px;
  font-size: 0.9rem;
  color: #526166;
}

.field input {
  border: 1px solid #cfd9d6;
  border-radius: 12px;
  padding: 12px 14px;
  font-size: 1rem;
  background: #f9fbfa;
  transition: border 0.2s ease, box-shadow 0.2s ease;
}

.field input:focus {
  outline: none;
  border-color: #7fbda5;
  box-shadow: 0 0 0 3px rgba(127, 189, 165, 0.2);
}

.actions {
  margin-top: 8px;
}

.submit {
  width: 100%;
  border: none;
  border-radius: 14px;
  padding: 12px 16px;
  font-size: 1rem;
  font-weight: 600;
  color: #1d2a2d;
  background: linear-gradient(135deg, #90c9b2, #b7d9c9);
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.submit:disabled {
  cursor: not-allowed;
  opacity: 0.8;
}

.submit:not(:disabled):hover {
  transform: translateY(-1px);
  box-shadow: 0 10px 20px rgba(127, 189, 165, 0.3);
}
</style>
