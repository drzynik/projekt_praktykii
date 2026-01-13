<template>
    <div class="inner">
      <div v-if="lastMessage" class="message-box">
        <p class="message-type">
          {{ lastMessage?.type }}
        </p>

        <div class="message-content">
          <p class="message-name">
            {{ lastMessage?.name }}:
          </p>

          <p class="message-text">
            {{ lastMessage?.message }}
          </p>
        </div>
      </div>
    </div>
</template>


<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from "vue";

const messages = ref([]);
let eventSource = null;

const lastMessage = computed(() => {
  return messages.value.length
    ? messages.value[messages.value.length - 1]
    : null;
});

onMounted(() => {
  eventSource = new EventSource("http://localhost:3000/gm/stream");

  eventSource.onmessage = (event) => {
    try {
      const data = JSON.parse(event.data);
      messages.value.push(data);
    } catch (e) {
      console.error("Invalid JSON:", e);
    }
  };

  eventSource.onerror = (err) => {
    console.error("SSE error:", err);
    eventSource.close();
  };
});

onBeforeUnmount(() => {
  if (eventSource) {
    eventSource.close();
  }
});
</script>
<style scoped>

.inner {
  height: 25vh;
  max-height: 100vh;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding:2%;
}

.message-box {
  margin-left: auto;
  margin-right: auto;
  width: 35vw;
  max-width: 28rem;
  background-color: #1a2130;
  padding: 1.5rem;
  border-radius: 1rem;
  border:1px solid #c4b5fd;
  box-shadow:1px 1px 5px #ddd6fe;
  text-transform: capitalize;
}

.message-type {
  color: whitesmoke;
  font-size: 20px;
  font-weight: 600;
  padding: 0.5rem;
}

.message-content {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 0.75rem;
}

.message-name {
  color: whitesmoke;
  font-size: 1rem;
  font-weight: 600;
  padding: 0.5rem;
}

.message-text {
  color: whitesmoke;
  font-size: 1rem;
  padding: 0.5rem;
  max-width: 200px;
}

</style>