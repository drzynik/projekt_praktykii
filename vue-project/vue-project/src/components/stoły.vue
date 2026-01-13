<template>
  <div>
    <header>
      <p>{{ page.h1 }}</p>
    </header>

    <section>
      <aside id="lewy"></aside>

      <main>
        <div id="wyszukiwanieKart">
          <img
            src="/src/assets/358016-200.png"
            alt="lupa"
            id="lupa"
            @click="toggleSearchBar"
          />

          <transition name="slide">
            <input
              v-if="showSBar"
              type="search"
              id="searchBar"
              v-model="searchQuery"
              placeholder="Wyszukaj kartę"
            />
          </transition>
        </div>

        <br>

        <div class="container">
          <div v-if="loading" class="loading">Ładowanie danych...</div>
          <div v-if="error" class="error">Błąd: {{ error }}</div>

          <div v-if="!loading && !error">
            <div 
              v-for="(d, index) in filteredDecisions"
              :key="index"
              class="decision-card"
            >

              <div class="header">
                <span class="card-id">
                  Karta ID: <span v-html="highlight(d.cardId.toString())"></span>
                </span>
              </div>
              <p class="pek">
                <strong>Krótki opis: </strong>
                <span v-html="highlight(d.shortDesc)"></span>
              </p>

              <p class="pek">
                <strong>Długi opis: </strong>
                <span v-html="highlight(d.longDesc)"></span>
              </p>
            </div>

            <div class="brak" v-if="filteredDecisions.length === 0">
              Brak kart pasujących do wyszukiwania
            </div>
          </div>
        </div>

        <br>
        <Button />
      </main>

      <aside id="prawy"></aside>
    </section>

    <footer>
      <img src="/src/assets/ITM_poziom_biale-C0cGn7Bx.png" alt="logo itm" id="logoITM" />
    </footer>
  </div>
</template>

<script>
import Button from './Button.vue';

export default {
  name: "Stoły",
  components: { Button },
  props: {
    page: {
      type: Object,
      default: () => ({ h1: "Karty do gry" }),
    },
  },
  data() {
    return {
      showSBar: false,
      searchQuery: "",
      decisions: [],
      loading: false,
      error: null
    };
  },
  computed: {
    filteredDecisions() {
      const q = this.searchQuery.toLowerCase();
      return this.decisions.filter(d => {
        const id = d.cardId?.toString().toLowerCase() || "";
        const short = d.shortDesc?.toLowerCase() || "";
        const long = d.longDesc?.toLowerCase() || "";
        return id.includes(q) || short.includes(q) || long.includes(q);
      });
    }
  },
  mounted() {
    window.addEventListener("storage", this.onStorageChange);
  },
  beforeUnmount() {
    window.removeEventListener("storage", this.onStorageChange);
  },
  methods: {
    toggleSearchBar() {
      this.showSBar = !this.showSBar;
      if (!this.showSBar) this.searchQuery = "";
    },
    fetchDecisions(deckName) {
      if (!deckName) return;
      this.loading = true;
      this.error = null;

      fetch(`https://localhost:7216/api/admin/deck/decisions-by-deck?deckName=${deckName}`)
        .then(async res => {
          if (!res.ok) {
            const text = await res.text();
            this.error = text;
            this.loading = false;
            return null;
          }
          return res.json();
        })
        .then(data => {
          if (data) this.decisions = data;
          this.loading = false;
        })
        .catch(err => {
          this.error = err.message;
          this.loading = false;
        });
    },
    onStorageChange(e) {
      if (e.key === "lastDeck") {
        const deck = e.newValue;
        if (deck) this.fetchDecisions(deck);
      }
    },
    highlight(text) {
      if (!this.searchQuery) return text;

      const escaped = this.searchQuery.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
      const regex = new RegExp(escaped, "gi");

      return text.replace(regex, match => `<span class="highlight">${match}</span>`);
    },

  }
};
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

  :deep(.highlight) {
    background-color:#a78bfa;
    color: black;
    padding: 0 2px;
    border-radius: 3px;
  }

  body{
    background-color: #0f172a;
    overflow: hidden;
    display: flex;
    flex-direction: column;
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

  footer{
    height:8vh;
    background-color: #141c2e;
    border-top:4px solid rgba(167, 139, 250, 0.4);
    position: relative;
  }

  section{
    width:100%;
    display:flex;
    justify-content: center;
    align-items: stretch;
    flex: 1;
    overflow: hidden;
  }

  header p{
    text-align: center;
    display: table-cell;
    vertical-align: middle;
    font-size: 2rem;
  }

  main{
    width:70%;
    position: relative;
    overflow-y: auto;
    padding-bottom:20px;
  }

  #lewy, #prawy{
    background-color: #161d2e;
    width:15%;
  }

  #searchBar{
    background-color: #192032;
    border:2px solid rgba(167, 139, 250, 0.2);
    border-radius:5px;
    height:3vh;
  }

  #searchBar:hover{
    border:2px solid lch(64.63% 62.35 303.68 / 0.4);
  }

  #wyszukiwanieKart{
    display: flex;
    margin:1%;
    gap: 1%;
    float: right;
    height:3vh;
  }

  #lupa{ 
    cursor: pointer;
    height:3vh;
  }

  .slide-enter-active, .slide-leave-active {
    transition: all 300ms ease;
  }

  .slide-enter-from, .slide-leave-to {
    width: 0;
    opacity: 0;
    padding: 0;
    margin: 0;
  }

  .slide-enter-to, .slide-leave-from {
    width: 200px;
    opacity: 1;
  }

  .container {
    max-width: 60vw;
    margin: 30px;
    font-size: 10px;
    overflow-y: auto;
    height:64.5vh;
    display: flex;
    flex-direction: column;
    gap: 15px;
  }

  .loading {
    font-size: 18px;
    color: #555;
  }

  .error {
    padding: 10px;
    background: #ffdddd;
    border-left: 4px solid #ff4444;
    margin-bottom: 20px;
  }

  .decision-card {
    border: 1px solid #ddd;
    border-radius: 8px;
    background-color: #080e1b;
    transition: 0.2s;
    padding: 12px;
    width: 98%;
    height: auto;
    display: flex;
    flex-direction: column;
    gap: 8px;
    margin:1%;
  }

  .decision-card:hover{
    box-shadow:1px 1px 10px #ddd6fe;
  }
  
  .header {
    font-weight: bold;
  }

  .card-id {
    color: whitesmoke;
    font-size:0.7rem;
  }

  strong{
    color:whitesmoke;
  }

  .pek{
    font-size: 0.7rem;
    line-height: 1.3rem;
  }

  .brak{
    font-size: 1rem;
    text-align: center;
    margin-top: 20px;
  }

  @media (max-width: 768px) {
  .container {
    max-width: 80vw;
    margin: 10px auto;
  }

  .decision-card {
    padding: 10px;
  }

  .card-id,
  .pek {
    font-size:0.7rem;
  }
}

</style>