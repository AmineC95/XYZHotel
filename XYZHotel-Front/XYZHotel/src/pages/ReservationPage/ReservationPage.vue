<template>
  <div class="reservation-container items-center q-px-xl">
    <q-card class="q-px-xl">
      <q-card-section class="q-mx-xl">
        <q-form @submit.prevent="submitReservation" class="q-py-xl q-gutter-md">
          <q-input
            label="Customer ID"
            v-model="reservation.customer.id"
            required
          />
          <q-input
            label="Full Name"
            v-model="reservation.customer.fullName"
            required
          />
          <q-input
            label="Email"
            type="email"
            v-model="reservation.customer.email.value"
            required
          />
          <q-input
            label="Phone Number"
            v-model="reservation.customer.phoneNumber.number"
            required
          />
          <q-input
            label="Password Hash"
            type="password"
            v-model="reservation.customer.passwordHash"
            required
          />
          <q-input
            label="Room ID"
            v-model="reservation.room.id"
            required
          />
          <q-select
            label="Room Type"
            v-model="reservation.room.type"
            :options="roomTypeOptions"
            required
          />
          <q-input
            label="Price Per Night"
            type="number"
            v-model="reservation.room.pricePerNight.amount"
            required
          />
          <q-input
            label="Check In Date"
            type="date"
            v-model="reservation.checkInDate"
            required
          />
          <q-input
            label="Check Out Date"
            type="date"
            v-model="reservation.checkOutDate"
            required
          />
          <q-input
            label="Number of Nights"
            type="number"
            v-model="reservation.numberOfNights"
            required
          />
          <q-select
            label="Status"
            v-model="reservation.status"
            :options="statusOptions"
            required
          />
          <q-btn label="Submit" type="submit" color="primary" />
        </q-form>
      </q-card-section>
    </q-card>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { createReservation } from "../../api/services/reservationApi";
import { Reservation, ReservationStatus, RoomType, Currency } from "../../api/models/index";

const reservation = ref<Reservation>({
  customer: {
    id: '',
    fullName: '',
    email: {
      value: '',
    },
    phoneNumber: {
      number: '',
    },
    passwordHash: '',
  },
  room: {
    id: '',
    type: RoomType.Value0,
    pricePerNight: {
      amount: null,
      currency: Currency.Value0,
    },
  },
  checkInDate: '',
  checkOutDate: '',
  numberOfNights: 0,
  status: ReservationStatus.Value0,
});

const statusOptions = Object.values(ReservationStatus).filter(value => typeof value === 'number');
const roomTypeOptions = Object.values(RoomType).filter(value => typeof value === 'number');

const submitReservation = async () => {
  try {

    reservation.value.status = Number(reservation.value.status);
    reservation.value.room.type = Number(reservation.value.room?.type);

    const response = await createReservation(reservation.value);
    console.log('Reservation Created:', response);
  } catch (error) {
    console.error('Error creating reservation:', error);
  }
};
</script>

