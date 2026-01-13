<template>
  <div>
    <header><p>Wybierz panel aplikacji</p></header>
    <section id="pick">
      <div id="player">
        <div id="gameMaster" class="wybor" @click="showLogin = true">
          <p>{{ page.gameMaster }}</p>
        </div>

        <div id="gracz" class="wybor" @click="showRoom = true">
          <p>{{ page.gracz }}</p>
        </div>
      </div>
    </section>
    <footer>
      <img src="/src/assets/ITM_poziom_biale-C0cGn7Bx.png" alt="logo itm" />
    </footer>

    <!-- Modal logowania -->
    <div v-if="showLogin" class="modal-overlay">
      <div class="modal">
        <h2>Zaloguj się</h2>

        <form class="inputy" @submit.prevent="sprawdzForm">
          <div id="field">
            <input type="text" placeholder="Nazwa użytkownika" v-model="username" />
            <p v-if="bladNazwa" style="color:red">{{ bladNazwa }}</p>
          </div>

          <div id="field">
            <input type="email" placeholder="Email" v-model="email" />
            <p v-if="bladEmail" style="color:red">{{ bladEmail }}</p>
          </div>

          <div id="field">
            <input type="password" placeholder="Hasło" v-model="password" />
            <p v-if="bladHaslo" style="color:red">{{ bladHaslo }}</p>
          </div>
          <div class="actions">
            <button @click="showLogin = false">Zamknij</button>
            <button type="submit">Wyślij</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Modal gracza -->
    <div v-if="showRoom" class="modal-overlay">
      <div class="modal">
        <h2>Podaj numer stołu</h2>
        <div id="field">
          <input type="text" placeholder="Numer stołu" v-model="numerStolu" />
          <p v-if="bladStol" style="color:red">{{ bladStol }}</p>
        </div>
        <div class="actions">
          <button @click="showRoom = false">Zamknij</button>
          <button @click="sprawdzStol">Dołącz</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>

export default {
  name: "Gra",
  props: {
    page: {
      type: Object,
      default: () => ({
        gameMaster: "Game Master",
        gracz: "Gracz",
      }),
    },
  },
  data() {
    return {
      userId: null,
      showLogin: false,
      showRoom: false,
      username: "",
      email: "",
      password: "",
      bladNazwa: "",
      bladHaslo: "",
      bladEmail: "",
      numerStolu: "",
      bladStol: "",
    };
  },
  
  methods: {
    sprawdzForm() {
      this.bladNazwa = "";
      this.bladHaslo = "";
      this.bladEmail = "";

      let valid = true;

      if (!this.username) {
        this.bladNazwa = "Należy wpisać nazwę użytkownika";
        valid = false;
      }

      if (!this.password) {
        this.bladHaslo = "Należy wpisać hasło";
        valid = false;
      }

      if (!this.email) {
        this.bladEmail = "Należy wpisać adres email";
        valid = false;
      }

      if(valid){
        this.userInfo();
      }
    },

    sprawdzStol() {
      this.bladStol = "";
      if (!this.numerStolu) {
        this.bladStol = "Należy podać numer stołu";
      } else if (isNaN(this.numerStolu)) {
        this.bladStol = "Numer stołu musi być liczbą";
      } else {
        this.$router.push("/stoly");
        this.showRoom = false;
      }
    },

    async userInfo() {
      try {
        const res = await fetch("https://localhost:7216/api/admin/deck/login", {
          method: "POST",
          body: this.createFormData()
        });

        const data = await res.json();

        if (data.userId) {

          console.log("Logowanie udane! Otrzymano userId:", data.userId);

          localStorage.setItem("userId", data.userId);
          this.$router.push("/gamemaster");
        } else {
          this.bladEmail = "Błędne dane logowania";

        }

      } catch (err) {
        console.error("Błąd:", err);
      }
    },

    createFormData() {
      const fd = new FormData();
      fd.append("email", this.email);
      fd.append("username", this.username);
      fd.append("password", this.password);
      return fd;
    }


  },
};
</script>

<style>
  @import url('https://fonts.googleapis.com/css2?family=Orbitron:wght@400..900&display=swap');

  *{
    margin:0;
    padding: 0;
    box-sizing: border-box;
    font-family: "Orbitron", sans-serif;
    font-optical-sizing: auto;
    font-style: normal;
    color:whitesmoke;
  }

  body{
    background-color: #0f172a;
  }

  header{
    height:11vh;
    width:100%;
    background-color: #141c2e;
    border-bottom:4px solid rgba(167, 139, 250, 0.4);
    display: table;
  }

  #pick{
    height:81vh;
    width:100%;
    display:flex;
    justify-content: center;
    align-items: center;
  }

  #player{
    height: auto;
    width:auto;
    display: flex;
    border: 2px solid rgba(167, 139, 250, 0.8);
    border-radius: 10px;
  }

  .wybor{
    height:40vh;
    width:30vw;
    border:2px solid rgba(167, 139, 250, 0.4);
    border-radius: 5px;
    margin:1%;
    display:table;
  }

  .wybor:hover{
    background-color: rgb(167, 139, 250);
    transition: 0.5s;
    transform: scale(1.02);
    cursor: pointer;
  }

  p{
    text-align: center;
    display: table-cell;
    vertical-align: middle;
    font-size: 2rem;
  }

  footer{
    height:8vh;
    background-color: #141c2e;
    border-top:4px solid rgba(167, 139, 250, 0.4);
    position: relative
  }

  img{
    height:7vh;
  }

  .modal-overlay {
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
    padding: 20px;
    width: 90%;
    max-width: 400px;
    display: flex;
    flex-direction: column;

    background: #080d24;
    border-radius: 8px;
    border:1px solid #c4b5fd;
    box-shadow:1px 1px 10px #ddd6fe;
  }

  .actions {
    display: flex;
    justify-content: space-between;
    margin-top: 20px;
    gap: 10px;
  }

  button{
    flex: 1;
    padding: 10px;
    cursor: pointer;

    font-size: 0.7rem;
    border-radius: 5px;
    border:1px solid #a78bfa;
    background-color: #141c2e;
  }

  .inputy{
    display: flex;
    flex-direction: column;
    gap: 10px;
  }

  input{
    padding: 7px;
    width: 100%;

    background-color: #94a3b8;
    color:whitesmoke;
    border:1px solid #a78bfa;
    border-radius:5px;
    color:black;
  }

  h2{
    margin-bottom: 15px;
    text-align: center;
    font-size:2.5rem;
  }

  button:hover {
    opacity: 0.9;
    background-color: rgb(167, 139, 250);
    transition: 0.5s;
  }

  #field {
    display: flex;
    flex-direction: column;
    margin-bottom: 15px;
  }

  .inputy2{
    display: flex;
    flex-direction: column;
    margin-bottom: 5px;
  }

  #field p{
    font-size: 0.7rem;
    margin: 0;
  }

  @media (max-width: 480px) {
    .modal {
      width: 95%;
      padding: 15px;
    }

    .actions {
      flex-direction: column;
    }

    .actions button {
      width: 100%;
    }
  }
</style>