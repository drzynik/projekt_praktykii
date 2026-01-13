<template>
    <button id="reportBtn" @click="showModal = true">Zgłoś błąd</button>

    <div v-if="showModal" class="modal-backdrop">
      <div class="modal">
        <h3 style="text-align: center;">Zgłoś błąd</h3>

        <textarea
          v-model="description"
          placeholder="Opisz problem..."
        ></textarea>
        <input type="email" v-model="email" placeholder="Podaj email">

        <div class="actions">
          <button @click="sendReport">Wyślij</button>
          <button @click="closeModal" class="close">Anuluj</button>
        </div>

        <p v-if="error" class="error">{{ error }}</p>
        <p v-if="success" class="success">Zgłoszenie wysłane ✔</p>
      </div>
    </div>
</template>

<script setup>
import { ref } from "vue";

const showModal = ref(false);
const description = ref("");
const error = ref("");
const success = ref("");

const email = ref("");


const apiUrl = "http://localhost:5194/api/report";

const closeModal = () => {
  showModal.value = false;
  description.value = "";
  error.value = "";
  success.value = "";
};

const sendReport = async () => {
  error.value = "";
  success.value = "";

  if (!description.value) {
    error.value = "Opis błędu jest wymagany!";
    return;
  }

  try {
    const res = await fetch(apiUrl, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        description: description.value,
        email: email.value
      })
    });

    if (!res.ok) {
      const text = await res.text();
      error.value = "Błąd serwera: " + text;
      return;
    }

    success.value = "Zgłoszenie wysłane ✔";
    setTimeout(closeModal, 2000);
  } catch (e) {
    error.value = "Nie udało się wysłać zgłoszenia";
    console.error(e);
  }
};
</script>

<style scoped>
#reportBtn{
  border-radius:5px;
  padding:1%;
  position: absolute;
  bottom: 0;
  right:0;
  margin:1%;
}

button{
  background-color: rgb(167, 139, 250);
}

.close{
  background-color: #141c2e;
}

.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
  background: rgba(0,0,0,0.7);

}

.modal {
  position: absolute;
  left: 50%;
  width: 90%;
  max-width: 18rem;
  background-color: #141c2e;
  border-radius: 0.75rem;
  border:1px solid #c4b5fd;
  box-shadow:1px 1px 10px #ddd6fe;
  padding: 1rem;
  transform: translateX(-50%);
  animation: scaleFadeIn 0.3s ease forwards;
}

textarea {
  width: 100%;
  height: 25vh;
  margin-top: 5%;
}

input {
  width: 100%;
  margin-top: 10px;
  padding: 6px;
}

.actions {
  margin-top: 12px;
  display: flex;
  gap: 8px;
  justify-content: flex-end;
}

.error {
  color: red;
  margin-top: 8px;
  font-size: 1rem;
}

.success {
  color: green;
  margin-top: 8px;
  font-size: 1rem;
}
</style>


