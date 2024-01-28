<template>
  <q-page padding>
    <q-card class="q-mb-md">
      <q-card-section>
        <div class="text-h6">Réservations en cours</div>
        <div v-for="reservation in currentReservations" :key="reservation.id">
          <div>Nom du client : {{ reservation.Customer.FullName }}</div>
          <div>Date d'arrivée : {{ reservation.CheckInDate }}</div>
          <div>Date de départ : {{ reservation.CheckOutDate }}</div>
        </div>
      </q-card-section>
    </q-card>

    <!-- <q-card>
      <q-card-section>
        <div class="text-h6">Historique des réservations</div>
        <div v-for="reservation in pastReservations" :key="reservation.id">
          <div>Nom du client : {{ reservation.Customer.FullName }}</div>
          <div>Date d'arrivée : {{ reservation.CheckInDate }}</div>
          <div>Date de départ : {{ reservation.CheckOutDate }}</div>
        </div>
      </q-card-section>
    </q-card> -->
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import jwtDecode from "jwt-decode";
import {
  createReservation,
  getReservation,
} from "../../api/services/reservationApi";
import {
  Reservation,
  ReservationStatus,
  RoomType,
  Currency,
} from "../../api/models/index";

const currentReservations = ref([]);
const pastReservations = ref([]);

onMounted(async () => {
  const token = localStorage.getItem("token");
  const decodedToken = jwtDecode(token);
  const customerId = decodedToken.customerId;

  currentReservations.value = await getReservation(customerId);
  pastReservations.value = await apiGetPastReservations();
});
</script>
