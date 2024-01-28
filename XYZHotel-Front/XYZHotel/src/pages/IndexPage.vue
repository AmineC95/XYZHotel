<template>
  <q-page>
    <div class="q-pa-lg">
      <q-carousel
        class="q-px-xl"
        swipeable
        animated
        v-model="slide"
        thumbnails
        infinite
        @update:modelValue="closeInfoCard"
      >
        <q-carousel-slide
          :name="1"
          img-src="https://plus.unsplash.com/premium_photo-1661843652801-66305e3c980c?q=80&w=2037&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
        >
          <q-btn color="primary" @click="showMoreInfo(1)">
            Plus d'informations
          </q-btn>
        </q-carousel-slide>

        <q-carousel-slide
          :name="2"
          img-src="https://plus.unsplash.com/premium_photo-1681487479203-464a22302b27?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
        >
          <q-btn color="primary" @click="showMoreInfo(2)">
            Plus d'informations
          </q-btn>
        </q-carousel-slide>

        <q-carousel-slide
          :name="3"
          img-src="https://plus.unsplash.com/premium_photo-1661962495669-d72424626bdc?q=80&w=2071&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
        >
          <q-btn color="primary" @click="showMoreInfo(3)">
            Plus d'informations
          </q-btn>
        </q-carousel-slide>
      </q-carousel>
    </div>
    <div>
      <q-card v-if="showInfo" class="q-pa-lg">
        <q-card-section class="q-pt-none q-mt-none">
          <h6 class="q-mt-none">
            Informations pour le Slide {{ currentSlide }}
          </h6>
          <p>Type de chambre : {{ rooms[currentSlide - 1].Type }}</p>
          <p>
            Prix par nuit : {{ rooms[currentSlide - 1].PricePerNight?.Amount }}
            {{ rooms[currentSlide - 1].PricePerNight?.Currency }}
          </p>
          <p>Infos : {{ rooms[currentSlide - 1].Infos?.join(", ") }}</p>
          <q-btn color="primary" @click="goToNextPage(rooms[currentSlide - 1])"
            >Aller à l'autre page</q-btn
          >
        </q-card-section>
      </q-card>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { getRooms } from "../../src/api/services/api";
import { Room } from "../../src/api/models/index";

const slide = ref(1);
const showInfo = ref(false);
const currentSlide = ref(0);
const rooms = ref<Room[]>([]);
const router = useRouter();
const isLoggedIn = ref(!!localStorage.getItem("token"));

onMounted(async () => {
  rooms.value = await getRooms();
});

function closeInfoCard() {
  showInfo.value = false;
}

function showMoreInfo(slideNumber: number) {
  currentSlide.value = slideNumber;
  showInfo.value = true;
}

function goToNextPage(room: Room) {
  // Convertir l'objet room en une chaîne JSON
  const roomString = JSON.stringify(room);

  // Stocker la chaîne JSON dans le localStorage
  localStorage.setItem('room', roomString);

  // Naviguer vers la page suivante
  if (isLoggedIn.value) {
    router.push({ name: 'Dashboard' });
  } else {
    router.push({ name: "Login" });
  }
  console.log("Going to next page", room);
}
</script>
