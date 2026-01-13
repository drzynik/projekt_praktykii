<template>
      <!-- Button -->
  <button id="zawołanieGamemastera" @click="open = !open">
    Przywołaj gamemastera
  </button>

    <div v-if="open" class="panel-backdrop">
      <div class="panel">
        <form @submit.prevent="send" class="form">
          <!-- Imię -->
          <input
            v-model="form.name"
            type="text"
            placeholder="Twoje imię"
            required
          />

          <!-- Typ zgłoszenia -->
          <select v-model="form.type" required>
            <option disabled value="">Wybierz typ zgłoszenia</option>
            <option value="pytanie">Pytanie</option>
            <option value="problem">Problem</option>
            <option value="fabula">Fabularne</option>
          </select>

          <!-- Wiadomość -->
          <textarea
            v-model="form.message"
            placeholder="Napisz wiadomość do gamemastera..."
            required
          ></textarea>

          <!-- Submit -->
          <div class="buttons">
            <button type="submit" class="submit">Wyślij</button>
            <button @click="closePanel">Anuluj</button>
          </div>
        </form>
      </div>  
    </div>
</template>

<script setup>
import { ref } from "vue";
import axios from "axios";

const open = ref(false);

const url = "http://localhost:3000/message";


const form = ref({
  name: "",
  type: "",
  message: "",
});

const sendRequest = async (url, data) => {
  const response = await axios.post(url, data);
  return response;
};

function closePanel() {
  open.value = false;
}


const send = async () => {
  console.log("Formularz wysłany:", form.value);
  const sendToGamemaster = await sendRequest(url, form.value);
  console.log(sendToGamemaster);
  // reset
  form.value = {
    name: "",
    type: "",
    message: "",
  };

  open.value = false;
};
</script>

<style>
#zawołanieGamemastera {
  background-color: rgb(167, 139, 250);
  border-radius: 5px;
  padding: 1%;
  position: absolute;
  bottom: 0;
  right: 0;
  margin: 1%;
  transition: all 0.3s ease;
  cursor: pointer;
}

.panel {
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

.panel-backdrop {
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

.form {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

input, select, textarea {
  width: 100%;
  border: 1px solid #d1d5db;
  border-radius: 0.5rem;
  padding: 0.5rem;
  outline: none;
  transition: box-shadow 0.2s ease;
  font-size: 1rem;
}

option {
  background-color: #141c2e;
}


textarea {
  height: 6rem;
  resize: none;
}

.submit {
  width: 100%;
  padding: 0.5rem;
  background-color: rgb(167, 139, 250);
  color: white;
  border-radius: 0.5rem;
  transition: background-color 0.3s ease;
  cursor: pointer;
}

@keyframes scaleFadeIn {
  from {
    opacity: 0;
    transform: translateX(-50%) scale(0.9);
  }
  to {
    opacity: 1;
    transform: translateX(-50%) scale(1);
  }
}


@media (max-width: 480px) {
  .panel {
    width: 18rem;
    max-width: none;
    border-radius: 0.5rem;
    padding: 0.75rem;
  }

  input, select, textarea {
    font-size: 0.9rem;
    padding: 0.4rem;
  }

  .submit {
    padding: 0.45rem;
  }

  textarea {
    height: 5rem;
  }
}


@media (min-width: 768px) {
  .panel {
    width: 22rem;
    max-width: 90%;
  }

  input, select, textarea {
    padding: 0.6rem;
  }

  .submit {
    padding: 0.6rem;
  }

  textarea {
    height: 7rem;
  }
}

@media (min-width: 1024px) {
  .panel {
    width: 18rem;
  }
}

.buttons{
  display: flex;
  gap:3%;
}

</style>

