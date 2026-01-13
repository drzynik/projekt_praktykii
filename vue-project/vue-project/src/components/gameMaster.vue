<template>
  <div>
    <header>
      <p>{{ page.h1 }}</p>
    </header>

    <section>
      <aside id="lewy"></aside>

      <main>
          <div id="przyciski">
            <p>Wybierz plik</p>
            <form @submit.prevent="registerAnswer" id="formularz">
              <input @change="updateVal" type="file" id="file">
              <button type="submit" id="submit">
                Zaimportuj talię kart
              </button>
            </form>
              
            <br />

            <p>Wybierz talię kart</p>
            <form id="sendDeckForm">
              <select id="select" v-model="selectedDeck">
                <option disabled value="">-- wybierz talię --</option>
                <option
                  v-for="deck in decks"
                  :key="deck.id"
                  :value="deck.name.replace('.pdf', '')"
                >
                  {{ deck.name }}
                </option>
              </select>
              <button id="sendDeck" @click.prevent="sendDeck">Wyślij talię kart</button>
            </form>
            <Okienko />

          </div>
        <BugReportButton />
      </main>

      <aside id="prawy"></aside>
      
    </section>

    <footer>
      <img src="/src/assets/ITM_poziom_biale-C0cGn7Bx.png" alt="logo itm" id="logoITM" />
    </footer>
  </div>
</template>

<script>
import BugReportButton from './BugReportButton.vue';
import Okienko from './okienko.vue';
  
export default {
  name: "gameMaster",
  components:{
    BugReportButton,
    Okienko
  },
  props: {
    page: {
      type: Object,
      default: () => ({
        h1: "Aplikacja Game Mastera",
      }),
    },
  },
  data() {
    return {
      popUpWindow: false,
      fileInp: null,
      decks: [],
      selectedDeck: ""
    };
  },
  methods: {
    registerAnswer() {
      if (!this.fileInp) return;

      const formData = new FormData();
      formData.append("file", this.fileInp);

      const userId = localStorage.getItem("userId");
      formData.append("userId", userId);

      fetch("https://localhost:7216/api/admin/deck/upload", {
        method: "POST",
        body: formData
      })
        .then(res => res.json())
        .then(data => {
          this.decks.push({
            id: data.DeckId,
            name: this.fileInp.name
          });

          this.selectedDeck = data.DeckId;
        })
        .catch(err => console.error("Fetch error:", err));
      },
      
    updateVal(e) {
      this.fileInp = e.target.files[0];
      this.checkFile = e.target.value;
    },

    sendDeck() {
      if (!this.selectedDeck) return;
      localStorage.setItem("lastDeck", "");
      localStorage.setItem("lastDeck", this.selectedDeck);
    }


  }
}
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Orbitron:wght@400..900&display=swap');

    *{
        margin: 0;
        padding:0;
        box-sizing: border-box;
        font-family: "Orbitron", sans-serif;
        color:whitesmoke;
        font-optical-sizing: auto;
        font-style: normal;
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

    #logoITM{
        height:7vh;
    }

    #sendDeck{
      background-color: rgb(167, 139, 250);
        border-radius:0 10px 10px 0;
        padding:1%;
        height:4vh;
    }

    #sendDeckForm{
      display: flex;
      width:30vw;
    }

    footer{
        height:8vh;
        background-color: #141c2e;
        border-top:4px solid rgba(167, 139, 250, 0.4);
        position: relative;
    }

    section{
        height:81vh;
        width:100%;
        display:flex;
        justify-content: center;
        align-items: center;
    }

    p{
        text-align: center;
        display: table-cell;
        vertical-align: middle;
        font-size: 2rem;
    }

    main{
        width:70%;
        height:81vh;
        position: relative;
    }

    #lewy, #prawy{
        background-color: #161d2e;
        width:15%;
        height:81vh;
    }


    button:hover{
        cursor: pointer;
    }

    #submit{
        background-color: rgb(167, 139, 250);
        border-radius:0 10px 10px 0;
        font-size: 1rem;
        padding:1%;
    }

    #file{
      border-radius: 10px 0 0 10px;
      width:70%;
      font-size: 1rem;
      padding:1%;
    }

    #przyciski{
        display:flex;
        justify-content: center;
        align-items: center;
        margin:2%;
        flex-direction: column;
    }

    #select{
        font-size: 0.7rem;
        border-radius: 5px 0px 0px 5px;
        border:1px solid rgba(167, 139, 250, 0.4);
        background-color: #141c2e;
        height:4vh;
        width:70%;
    }

    #formularz{
      display: flex;
    }

    @media(max-width:1000px){
      #submit, #file{
        font-size:0.7rem;
      }
    }
</style>