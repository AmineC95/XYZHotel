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

        <q-carousel-slide
          :name="4"
          img-src="https://plus.unsplash.com/premium_photo-1682093007363-b05f4c3dc932?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
        >
          <q-btn color="primary" @click="showMoreInfo(4)">
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
        </q-card-section>
      </q-card>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from "vue";

const slide = ref(1);
const showInfo = ref(false);
const currentSlide = ref(0);

function showMoreInfo(slideNumber) {
  currentSlide.value = slideNumber;
  showInfo.value = true;
}
</script>
