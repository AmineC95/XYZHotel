<template>
  <q-page padding>
    <q-card class="q-mb-md">
      <q-card-section>
        <div class="text-h6">Réservations en cours</div>
        <div v-for="reservation in currentReservations" :key="reservation.id">
          <!-- Affichez les détails de la réservation ici -->
        </div>
      </q-card-section>
    </q-card>

    <q-card>
      <q-card-section>
        <div class="text-h6">Historique des réservations</div>
        <div v-for="reservation in pastReservations" :key="reservation.id">
          <!-- Affichez les détails de la réservation ici -->
        </div>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { createReservation } from "../../api/services/reservationApi";
import { Reservation, ReservationStatus, RoomType, Currency } from "../../api/models/index";

const currentReservations = ref([]);
const pastReservations = ref([]);

onMounted(async () => {
  currentReservations.value = await apiGetCurrentReservations();
  pastReservations.value = await apiGetPastReservations();
});
</script>

