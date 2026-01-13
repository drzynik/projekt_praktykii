import { createRouter, createWebHistory } from "vue-router";
import Gra from "./components/gra.vue";
import GameMaster from "./components/gameMaster.vue";
import Stoły from "./components/stoły.vue";

const routes = [
  { path: "/", name: "Gra", component: Gra },
  { path: "/gamemaster", name: "GameMaster", component: GameMaster },
  { path: "/stoly", name: "Stoły", component: Stoły },
];


const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

